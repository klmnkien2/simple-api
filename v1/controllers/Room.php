<?php

namespace v1\controllers;

class Room extends \SlimController\SlimController
{
    private $model, $serverModel, $userModel;

    public function __construct(\Slim\Slim &$app) {
        parent::__construct($app);
        $this->model = new \models\Room();
        $this->serverModel = new \models\Server();
        $this->userModel = new \models\User();
    }

    /**
     * PUBLIC FUNCTION - ROUTER ACTION
     */
    public function createAction()
    {
        // reads multiple params only if they are POST
        $room_name = $this->param('room_name', 'post');
        $create_user = $this->param('create_user', 'post');
        // Get old room
        $currentRoom = $this->userModel->currentRoom($create_user);
        // leave current room
        if (!empty($currentRoom)) {
            $this->leaveRoom($currentRoom['room_id'], $currentRoom['user_id']);
        }

        $server_info = $this->getServerInfo();
        $room = array(
            'room_name' => $room_name,
            'create_user' => $create_user,
            'server_id' => $server_info['server_id'],
            'host_ip' => $server_info['host_ip']
        );

        $result = $this->model->createRoom($room);
        if ($result) {			
			$room['room_id'] = $result;
			$room['hub'] = $server_info['hub'];
            $this->echoRespnse(200, array('room' => $room));
        } else {
            $this->echoRespnse(400, array('room' => $room));
        }
    }

    public function deleteAction()
    {
        // reads multiple params only if they are POST
        $room_id = $this->param('room_id', 'post');
        $result = $this->model->deleteRoom($room_id);
        if ($result) {
            $this->echoRespnse(200, array('deleted' => true));
        } else {
            $this->echoRespnse(400, array('deleted' => false));
        }
    }

    public function listAction()
    {
        $rooms = $this->model->getRoomList();
        $this->echoRespnse(200, array('rooms' => $rooms));
    }

    /**
     * @param room_id Required. ID of the room.
     */
    public function showAction()
    {
        $room_id = $this->param('room_id');

        $result = $this->model->getRoomById($room_id);
        if ($result) {
            $this->echoRespnse(200, array('room' => $result));
        } else {
            $this->echoRespnse(400, array('room' => false));
        }
    }

    /**
     * @param room_id Required. ID or name of the room.
     * @param from Required. Name the message will appear be sent from. Must be less than 15 characters long. May contain letters, numbers, -, _, and spaces.
     * @param message Required. The message body. 10,000 characters max.
     * @param message_format Determines how the message is treated by our server and rendered inside HipChat applications.
     *   html - Message is rendered as HTML and receives no special treatment. Must be valid HTML and entities must be escaped (e.g.: &amp; instead of &). May contain basic tags: a, b, i, strong, em, br, img, pre, code, lists, tables. Special HipChat features such as @mentions, emoticons, and image previews are NOT supported when using this format.
     *   text - Message is treated just like a message sent by a user. Can include @mentions, emoticons, pastes, and auto-detected URLs (Twitter, YouTube, images, etc).(default: html)
     * @param notify Whether or not this message should trigger a notification for people in the room (change the tab color, play a sound, etc). Each recipient's notification preferences are taken into account. 0 = false, 1 = true. (default: 0)
     */
    public function messageAction()
    {
        // reads multiple params only if they are POST
        $message = $this->params(
            array('room_id', 'user_id', 'user_name', 'receive_id', 'receive_name', 'notify', 'message'), 
            'post', 
            array(
                'room_id' => 0,
                'user_id' => 0,
                'user_name' => '',
				'receive_id' => 0,
				'receive_name' => '',
                'message' => '',
                'notify' => 0
            )
        );

        $result = $this->model->createMessage($message);
        if ($result) {
            $this->echoRespnse(200, array('status' => 'sent'));
        } else {
            $this->echoRespnse(400, array('status' => 'error'));
        }
    }

    /**
    * @param room_id Required. ID of the room.
    * @param date Required. Either the date to fetch history for in YYYY-MM-DD format, or "recent" to fetch the latest 50 messages.
    * @param timezone Your timezone. Must be a supported timezone. (default: UTC)
    */
    public function historyAction()
    {
        $params = $this->params(array('room_id', 'message_id', 'part'), 'get', array(
                'room_id' => 0, // default
                'message_id' => 0, // default
				'part' => 'new'
            ));
        $message_list = array();
		if ($params['part'] == 'new') {
			$message_list = $this->model->newMessageByRoom($params['room_id'], $params['message_id']);
		} else {
			$message_list = $this->model->oldMessageByRoom($params['room_id'], $params['message_id'], 50);
		}

        $this->echoRespnse(200, array('history' => $message_list));
    }

    public function inviteAction()
    {
        // reads multiple params only if they are POST
        $user_id = $this->param('user_id', 'post');
        $user_name = $this->param('user_name', 'post');
        $room_id = $this->param('room_id', 'post');
        $receivers = $this->param('receivers', 'post');
        $array_ids = split(",", $receivers);

        $total_success = 0;
        foreach ($array_ids as $id) {
            if (!$id) continue;
            $invite_message = array(
                'room_id' => 0,
                'user_id' => $user_id,
                'user_name' => $user_name,
				'receive_id' => $id,
				'receive_name' => '',
                'message' => "room_invitation:$room_id",
                'notify' => 1,
                'message_format' => 'text'
            );
            $result = $this->model->createMessage($invite_message);
            if ($result) {
                $total_success ++;
            }
        }

        if ($total_success) {
            $this->echoRespnse(200, array('inviteDone' => $total_success));
        } else {
            $this->echoRespnse(400, array('inviteDone' => 0));
        }
    }

    /**
     * PRIVATE FUNCTION
     */
    private function getServerInfo() {
        $info = array();

        $serverModel = new \models\Server();
        $server = $serverModel->getRandom();
        if ($server) {
            $info['server_id'] = $server['server_id'];
            $info['hub'] = $server['hub'];
            $info['host_ip'] = $server['host'] . ':' . $server['port'];
        }

        return $info;
    }

    private function leaveRoom($room_id, $user_id)
    {
        $users = $this->userModel->getUserInRoom($room_id);
        if (empty($users)) {
            return 0;
        }

        $result = $this->userModel->deleteRoomUser($room_id, $user_id);
        if ($result == 0) {
            return 0;
        }

        if (count($users) == 1) { // last user in room. remove room
            $r1 = $this->serverModel->unbindServer($room_id);
            $r2 = $this->model->deleteRoom($room_id);
            if($r1 && $r2) {
                return 2;//2-also delete room
            }
        }

        return 1;//1-only delete room_users
    }
}
?>