<?php

namespace v1\controllers;

class User extends \SlimController\SlimController
{

    private $model, $serverModel, $roomModel;

    public function __construct(\Slim\Slim &$app) {
        parent::__construct($app);
        $this->model = new \models\User();
        $this->serverModel = new \models\Server();
        $this->roomModel = new \models\Room();
    }

    public function loginAction()
    {
        try {
            // reads multiple params only if they are POST
            $username = $this->param('username', 'post');
            $password = $this->param('password', 'post');
            $state = $this->param('state', 'post');
    
            // Call API
            $postdata = http_build_query(
                array(
                    'username' => $username, 
                    'password' => $password
                )
            );
    
            $opts = array('http' =>
                array(
                    'method'  => 'POST',
                    'header'  => 'Content-type: application/x-www-form-urlencoded',
                    'content' => $postdata
                )
            );
            $context  = stream_context_create($opts);
            $result = file_get_contents('http://trading.gametv.vn/api_platform/app_login', false, $context);
            //$result = '{"status":1}';
            $response = array();
            $result = json_decode($result, true);
    
            if($result['status'] != 1) {
                $response['error'] = $result['message'];
                $this->echorespnse(400, $response);
            } else {
                $result = $result['data'];
                $response['user_id'] = isset($result['id']) ? $result['id'] : null;
                $response['avatar'] = isset($result['avatar']) ? $result['avatar'] : '';
                $response['level'] = isset($result['platform_level']) ? $result['platform_level'] : '';
                $response['diamond'] = isset($result['platform_kimcuong']) ? $result['platform_kimcuong'] : '';
                $response['needpay'] = isset($result['platform_expried']) ? $result['platform_expried'] : '';
                $response['username'] = $username;
                $response['password'] = $password;
                $response['state'] = is_null($state)?1:$state;
                //var_dump($response);die;
                $this->model->syncUserInfo($response);
                $response['password'] = '';//unset not to response

                $this->echorespnse(200, $response);
            }
        } catch(\Exception $ex) {
            //var_dump($ex);die;
            $this->echorespnse(400, array("error" => "Can't not connect to API."));
        }
    }
    
    public function paymentAction()
    {
//         http://trading.gametv.vn/api_platform/mobile_card/?ajax=ajax&user_id=1000015993&card_type=92&card_seri=11111111111111&card_code=22222222222222
//         + Card_seri
//         + Card_code
//         + Card_type(92 = Mobi,93 = Vina,107 = Viettel)
        try {
            // reads multiple params only if they are POST
            $user_id = $this->param('user_id', 'post');
            $card_seri = $this->param('card_seri', 'post');
            $card_code = $this->param('card_code', 'post');
            $card_type = $this->param('card_type', 'post');
    
            $api_url = "http://trading.gametv.vn/api_platform/mobile_card/?ajax=ajax&user_id=100001599&card_type=$card_type&card_seri=$card_seri&card_code=$card_code";

            $opts = array('http' =>
                array(
                    'method'  => 'GET'
                )
            );
            $context  = stream_context_create($opts);
            $result = file_get_contents($api_url, false, $context);
            $result = json_decode($result, true);

            $response = array();
    
            if(empty($result['error']) || !$result['error']) {
                $response['error'] = '';
                $this->echorespnse(200, $response);
            } else {
                $response['error'] = $result['error'];
                $this->echorespnse(400, $response);
            }
        } catch(\Exception $ex) {
            $this->echorespnse(400, array("error" => "Can't not connect to API."));
        }
    }

    private function updateLevel($user_id)
    {
        // Update level
        // http://trading.gametv.vn/api_platform/update_level/?ajax=ajax&id=3&level=12
//         Level
//         1 -> 10 | 1000 giờ
//         10 - 20 | 2000 giờ
//         20 - 30 | 3000 giờ
        // Update Gem
        // http://trading.gametv.vn/api_platform/update_gem/?ajax=ajax&id=3&gem=1200
        // ALTER TABLE `user_caches` ADD `level_hours` INT(11) NOT NULL DEFAULT '0' AFTER `level`;

        try {
            $hour_per_level = 100;

            $user = $this->model->getUserById($user_id);
            if ($user['taken_hours'] > 60 * 5) {
                // Neu user dang inactive thi out luon
                return;
            }

            $level = $user['level'];
            $level_hours = $user['taken_hours'] + $user['level_hours'];

            if ($level_hours > $hour_per_level * 3600) {
                $level = $level + 1;
                $level_hours = $level_hours - $hour_per_level * 3600;
            }
            // Database update
            $this->model->updateLevel($user_id, $level, $level_hours);

            // API update
            $api_url = "http://trading.gametv.vn/api_platform/update_level/?ajax=ajax&id=$user_id&level=$level";
            $opts = array('http' =>
                array(
                    'method'  => 'GET'
                )
            );
            $context  = stream_context_create($opts);
            $result = file_get_contents($api_url, false, $context);
            $result = json_decode($result, true);

            if(!empty($result['status']) && $result['status'] == true) {
                return true;
            }
        } catch(\Exception $ex) {
            //DO NOTHING HERE
            //$this->echorespnse(400, array("error" => "Can't not connect to API."));
        }

        return false;
    }
    
    public function logoutAction()
    {
        // reads multiple params only if they are POST
        $user_id = $this->param('user_id', 'post');

        $response = $this->model->updateLastLogin($user_id);
        
        $this->echoRespnse(200, array('success' => $response));
    }

    public function showAction()
    {
        $this->echoRespnse(200, array("message" => "API aren't installed"));
    }
    
    public function statusAction()
    {
        // reads multiple params only if they are POST
        $param = $this->params(
            array('user_id', 'state', 'status'), 
            'post', 
            array(
                'user_id' => null,
                'state' => null,
                'status' => null
            )
        );

        if (isset($param['user_id']) && $param['user_id']) {
            $this->model->updateUserInfo($param['user_id'],$param['state'], $param['status']);
            $this->echoRespnse(200, array('status' => 'updated'));
        }else {
            $this->echoRespnse(400, array('error' => 'no user'));
        }
    }
    
    public function friendListAction()
    {
        $user_id = $this->param('user_id');
        $this->updateLevel($user_id);
        $this->model->updateLastLogin($user_id);
        $friends = $this->model->getFriendList($user_id);
        $this->filterInactiveFriend($friends);
        $this->echoRespnse(200, array('friends' => $friends));
    }
    
    private function filterInactiveFriend(&$list) {
        $current = time();
        foreach ($list as &$friend) {
            $delay_minute = ($current - $friend['last_active'])/60;
            if ($delay_minute > 5) {
                $friend['state'] = "-1";
            }
        }
    }

    public function addFriendAction()
    {
        // reads multiple params only if they are POST
        $user_id = $this->param('user_id', 'post');
        $user_name = $this->param('user_name', 'post');
        $friend_name = $this->param('friend_name', 'post');
        $message = $this->param('message', 'post');

        $friend_id = $this->model->addFriend($user_id, $friend_name, $message);

        if($friend_id) {
            // NOTIFY MESSAGE
            $messageArr = array(
                'room_id' => 0,
                'notify' => 1,
                'user_id' => $user_id,
                'user_name' => $user_name,
                'receive_id' => $friend_id,
                'receive_name' => $friend_name,
                'message' => $message
            );
            $result = $this->roomModel->createMessage($messageArr);
            // RETURN RESPONSE
            if ($result>0) {
                $this->echoRespnse(200, array('friend_id' => $friend_id));
            }
            else {
                $this->echoRespnse(400,  array('success' => false, 'error' => 'MES=false'));
            }
            
        } else {
            // DEFAULT RETURN
            $this->echoRespnse(400,  array('success' => false, 'error' => 'CF=false'));
        }
    }
    
    public function updateFriendAction()
    {
        // reads multiple params only if they are POST
        $friend = $this->params(
            array('user1', 'user2', 'type', 'ignore_type'), 
            'post', 
            array(
                'user1' => null,
                'user2' => null,
                'type' => null,
                'ignore_type' => null
            )
        );

        if ($friend['type'] == -2) {
            $this->model->deleteFriend($friend['user1'], $friend['user2']);
        } else {
            $this->model->updateFriend($friend['user1'], $friend['user2'], $friend['type'], $friend['ignore_type']);
        }

        $this->echoRespnse(200, array('status' => 'done'));
    }
    
    public function roomAction()
    {
        $room_id = $this->param('room_id');
        $user_id = $this->param('user_id');
        $ip = $this->param('ip');
        $ping = $this->param('ping');

        $this->model->updateUserRoom($room_id, $user_id, $ip, $ping);
        $users = $this->model->getUserInRoom($room_id);
        $users = $this->filterInactiveMember($users);
        $this->echoRespnse(200, array('users' => $users));
    }
    
    private function filterInactiveMember($list) {
        $current = time();
        $output_list = array();
        foreach ($list as $user) {
            $delay_minute = ($current - $user['last_active'])/60;
            if ($delay_minute > 5) {
                $this->model->deleteRoomUser($user['room_id'], $user['user_id']);
                // [TODO] remove from vpn
            } else {
                $output_list[] = $user;
            }
        }
        return $output_list;
    }
    
    public function joinAction()
    {
        // reads multiple params only if they are POST
        $room_user = $this->params(
            array('room_id', 'user_id', 'user_name', 'room_name'), 
            'post', 
            array(
                'room_id' => 0,
                'room_name' => '',
                'user_id' => 0,
                'user_name' => ''
            )
        );

        // Get old room 
        $currentRoom = $this->model->currentRoom($room_user['user_id']);
        // leave current room
        if (!empty($currentRoom)) {
            $this->leaveRoom($currentRoom['room_id'], $room_user['user_id']);
            $this->model->decreaseMember($currentRoom['room_id']);
        }
        // create new room
        $test = $this->model->increaseMember($room_user['room_id']);
        if ($test) {
            // NOTIFY MESSAGE
            $messageArr = array(
                'room_id' => $room_user['room_id'],
                'notify' => 2, // 2= join room 1= add friend
                'user_id' => 0,
                'user_name' => '',
                'receive_id' => 0,
                'receive_name' => '',
                'message' => $room_user['room_name']
            );
            $this->roomModel->createMessage($messageArr);

            unset($room_user['room_name']); // khi insert ko co
            $result = $this->model->createRoomUser($room_user);
        } else {
            $result = false;
        }

        if ($result) {
            $this->echoRespnse(200, array('status' => 'ok'));
        } else {
            $this->echoRespnse(400, array('status' => 'error'));
        }
    }
    
    public function leaveAction()
    {
        // reads multiple params only if they are POST
        $room_user = $this->params(
            array('room_id', 'user_id'), 
            'post', 
            array(
                'room_id' => 0,
                'user_id' => 0
            )
        );

        $result = $this->leaveRoom($room_user['room_id'], $room_user['user_id']);

        if ($result) {
            $this->echoRespnse(200, array('status' => $result)); //1-only delete room_users, 2-also delete room
        } else {
            $this->echoRespnse(400, array('status' => 0));
        }
    }
    
    /**
    * @param room_id Required. ID of the room.
    * @param date Required. Either the date to fetch history for in YYYY-MM-DD format, or "recent" to fetch the latest 50 messages.
    * @param timezone Your timezone. Must be a supported timezone. (default: UTC)
    */
    public function historyAction()
    {
        $params = $this->params(array('user_id', 'receive_id', 'read_ids', 'last_view_id'), 'get', array(
                'user_id' => 0, // default
                'receive_id' => 0, // default
                'read_ids' => "", // must be string
                'last_view_id' => 0
            ));
        $message_list = array();
        if (!$params['receive_id']) {
            $message_list = $this->model->receiveAllMessage($params['user_id'], $params['read_ids']);
        } else {
            $message_list = $this->model->historyMessage($params['user_id'], $params['receive_id'], $params['last_view_id'], 20);
        }

        $this->model->updateLastLogin($params['user_id']);
        $this->echoRespnse(200, array('history' => $message_list));
    }

    //==================================================
    // PRIVATE FUNCTIONS
    //==================================================
    private function leaveRoom($room_id, $user_id)
    {
//         $users = $this->model->getUserInRoom($room_id);
//         if (empty($users)) {
//             return 0;
//         }

        $result = $this->model->deleteRoomUser($room_id, $user_id);
        if ($result == 0) {
            return 0;
        }

//         if (count($users) == 1) { // last user in room. remove room
//             $r1 = $this->serverModel->unbindServer($room_id);
//             $r2 = $this->roomModel->deleteRoom($room_id);
//             if($r1 && $r2) {
//                 return 2;//2-also delete room
//             } 
//         }

        return 1;//1-only delete room_users
    }
}
?>