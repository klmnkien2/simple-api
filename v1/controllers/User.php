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
    		//$result = file_get_contents('http://trading.gametv.vn/api_app/app_login', false, $context);
    		$result = '{"status":1}';
            $response = array();
    		$result = json_decode($result);
    
    		if($result->status != 1) {
    			$response['error'] = $result->message;
                $this->echorespnse(400, $response);
    		} else {
    		    $response['avatar'] = isset($result->avatar) ? $result->avatar : '';
    		    $response['level'] = isset($result->level) ? $result->level : '';
    		    $response['diamond'] = isset($result->diamond) ? $result->diamond : '';
    		    $response['needpay'] = isset($result->needpay) ? $result->needpay : '';
    		    $response['username'] = $username;
				$response['password'] = $password;
    		    $response['state'] = is_null($state)?1:$state;
    
    		    $this->model->syncUserInfo($response);
				$response['password'] = '';//unset not to response
    		    $this->echorespnse(200, $response);
    		}
        } catch(\Exception $ex) {
            //var_dump($ex);die;
            $this->echorespnse(400, array("error" => "Can't not connect to API."));
        }
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
		
        $friends = $this->model->getFriendList($user_id);
        $this->echoRespnse(200, array('friends' => $friends));
    }
	
	public function addFriendAction()
    {
        // reads multiple params only if they are POST
        $user_id = $this->param('user_id', 'post');
        $friend_name = $this->param('friend_name', 'post');

        $friend_id = $this->model->addFriend($user_id, $friend_name, $message);

        if(!$friend_id) {
            $this->echoRespnse(400,  array('success' => false));
        } else {
            $this->echoRespnse(200, array('friend_id' => $friend_id));
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
		$user_lan_ip = $this->param('user_lan_ip');
		
		$this->model->updateUserRoom($room_id, $user_id, $user_lan_ip);
        $users = $this->model->getUserInRoom($room_id);
        $this->echoRespnse(200, array('users' => $users));
    }
	
	public function joinAction()
    {
        // reads multiple params only if they are POST
        $room_user = $this->params(
            array('room_id', 'user_id', 'user_name', 'user_lan_ip'), 
            'post', 
            array(
                'room_id' => 0,
                'user_id' => 0,
                'user_name' => '',
				'user_lan_ip' => ''
            )
        );

        // Get old room 
        $currentRoom = $this->model->currentRoom($room_user['user_id']);
        // leave current room
        if (!empty($currentRoom)) {
            $this->leaveRoom($currentRoom['room_id'], $currentRoom['user_id']);
        }
        // create new room
        $result = $this->model->createRoomUser($room_user);

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
        $users = $this->model->getUserInRoom($room_id);
        if (empty($users)) {
            return 0;
        }

        $result = $this->model->deleteRoomUser($room_id, $user_id);
        if ($result == 0) {
            return 0;
        }

        if (count($users) == 1) { // last user in room. remove room
            $r1 = $this->serverModel->unbindServer($room_id);
            $r2 = $this->roomModel->deleteRoom($room_id);
            if($r1 && $r2) {
                return 2;//2-also delete room
            } 
        }

        return 1;//1-only delete room_users
    }
}
?>