<?php

/**
 * This is an example of User Class using PDO
 *
 */

namespace models;
use DbHandler\Core;
use PDO;

class User {

    protected $core;

    function __construct() {
        $this->core = Core::getInstance();
        //$this->core->dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    }

    // Get user by the Id
    public function getUserById($user_id) {
        $r = null;

        $sql = "SELECT * FROM user_caches WHERE user_id=:user_id";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':user_id', $user_id, PDO::PARAM_INT);

        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
        if (is_array($r) && !empty($r)) {
            $r = $r[0];
        }

        return $r;
    }

    // Get user by the Login
    public function getUserByLogin($username, $pass) {
        $r = array();

        $sql = "SELECT user_name, user_id, UNIX_TIMESTAMP(last_active) AS last_active  FROM user_caches WHERE user_name=:username";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':username', $username, PDO::PARAM_STR);

        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
		
		if (!empty($r)) {
			$r = $r[0];
		} else {
			$stmt = $this->core->dbh->prepare("INSERT INTO user_caches (user_name, password) VALUES (:user_name, :password)");
			$stmt->bindParam(':user_name', $username, PDO::PARAM_STR);
			$stmt->bindParam(':password', $pass, PDO::PARAM_STR);
			$stmt->execute();
		
			$r = array(
			    "user_name" => $username,
				"user_id" => $this->core->dbh->lastInsertId(),
				"last_active" => "0"
			);
		}

        return $r;
    }
    
    public function syncUserInfo(&$data) {

        $sql = "SELECT user_id,user_name,status,UNIX_TIMESTAMP(last_active) AS last_active FROM user_caches WHERE user_name=:username";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':username', $data['username'], PDO::PARAM_STR);

        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
    
        if (!empty($r)) {
            $r = $r[0];
            $data['user_id'] = $r['user_id'];
            $data['status'] = $r['status'];
			$data['last_active'] = $r['last_active'];

            //UPDATE
            $stmt = $this->core->dbh->prepare("UPDATE user_caches SET " .
                "user_name=:user_name," .
                "password=:password," .
                "avatar=:avatar," .
                "level=:level," .
                "diamond=:diamond," .
                "state=:state," .
                "last_active = now() " .
                "WHERE user_id = :user_id");
            $stmt->bindParam(':user_name', $data['username'], PDO::PARAM_STR);
            $stmt->bindParam(':password', $data['password'], PDO::PARAM_STR);
            $stmt->bindParam(':avatar', $data['avatar'], PDO::PARAM_STR);
            $stmt->bindParam(':level', $data['level'], PDO::PARAM_INT);
            $stmt->bindParam(':diamond', $data['diamond'], PDO::PARAM_INT);
            $stmt->bindParam(':state', $data['state'], PDO::PARAM_INT);
            $stmt->bindParam(':user_id', $r['user_id'], PDO::PARAM_INT);
            $stmt->execute();
        } else {
            //Insert
            $stmt = $this->core->dbh->prepare("INSERT INTO user_caches (user_name, password, avatar, level, diamond, state) " .
                "VALUES (:user_name, :password, :avatar, :level, :diamond, :state)");
            $stmt->bindParam(':user_name', $data['username'], PDO::PARAM_STR);
            $stmt->bindParam(':password', $data['password'], PDO::PARAM_STR);
            $stmt->bindParam(':avatar', $data['avatar'], PDO::PARAM_STR);
            $stmt->bindParam(':level', $data['level'], PDO::PARAM_INT);
            $stmt->bindParam(':diamond', $data['diamond'], PDO::PARAM_INT);
            $stmt->bindParam(':state', $data['state'], PDO::PARAM_INT);
            $stmt->execute();
        }
    }
    
    public function updateUserInfo($user_id, $state, $status) {
    
        $condition = "";
        $param = array();
        $param[':user_id'] = $user_id;
        
        if ($status!==null) {
            $condition .= " ,status=:status ";
            $param[':status'] = $status;
        }
        if ($state!==null) {
            $condition .= " ,state=:state ";
            $param[':state'] = $state;
        }
        
        //UPDATE
        $stmt = $this->core->dbh->prepare("UPDATE user_caches SET " .
            "last_active = now() " .
            $condition .
            "WHERE user_id = :user_id");
        $stmt->execute($param);
    }

	public function updateLastLogin($user_id) {
        $stmt = $this->core->dbh->prepare("UPDATE user_caches SET last_active = now() WHERE user_id = :user_id");
		$stmt->bindParam(':user_id', $user_id, PDO::PARAM_INT);
		
        if ($stmt->execute()) {
            return true;
        } else {
            return false;
        }
    }
    //=========================
    // BELLOW FOR FRIENDS QUERY
    //=========================

    public function addFriend($user_id, $friend_name) {    
        //CHECK IF USER EXIST OR NOT
        $sql = "SELECT user_id FROM user_caches WHERE user_name=:user_name";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':user_name', $friend_name, PDO::PARAM_STR);
        $friend_id = 0;
        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
            if(empty($r)) {
                return 0;
            } else {
                $friend_id = $r[0]['user_id'];
            }
        }
    
        // CHECK IF FRIEND ALREADY ADD ME OR NOT
        $sql = "SELECT user1 FROM friends WHERE user1=:user1 AND user2=:user2";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':user1', $friend_id, PDO::PARAM_INT);
        $stmt->bindParam(':user2', $user_id, PDO::PARAM_INT);
        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
            if(!empty($r)) {
                return 0;
            }
        }

        // IF CAN BE ADD THEN ADD
        $sql = "INSERT INTO friends (user1, user2)
				VALUES (:user1, :user2)";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':user1', $user_id, PDO::PARAM_INT);
        $stmt->bindParam(':user2', $friend_id, PDO::PARAM_INT);
    
        if ($stmt->execute()) {
            return $friend_id;
        } else {
            return 0;
        }
    
    }
    
    public function updateFriend($user1, $user2, $type, $ignore_type) {
        $stmt = $this->core->dbh->prepare("UPDATE friends SET type=:type, ignore_type=:ignore_type WHERE user1 = :user1 AND user2 = :user2 OR user1 = :user2 AND user2 = :user1");
        $stmt->bindParam(':user1', $user1, PDO::PARAM_INT);
        $stmt->bindParam(':user2', $user2, PDO::PARAM_INT);
        $stmt->bindParam(':type', $type, PDO::PARAM_INT);
        $stmt->bindParam(':ignore_type', $ignore_type, PDO::PARAM_INT);
        $stmt->execute();
    }
	
	public function deleteFriend($user1, $user2) {
        $stmt = $this->core->dbh->prepare("DELETE FROM friends WHERE user1 = :user1 AND user2 = :user2 OR user1 = :user2 AND user2 = :user1");
        $stmt->bindParam(':user1', $user1, PDO::PARAM_INT);
        $stmt->bindParam(':user2', $user2, PDO::PARAM_INT);
        $stmt->execute();
    }
	
	public function getFriendList($user_id) {
        $r1 = array();
        $r2 = array();

        // Get friend send request to me
        $sql = "SELECT f.*, u.user_name, u.user_id, u.avatar, u.status, u.state
        	FROM user_caches u
            LEFT JOIN friends f ON f.user1 = u.user_id
            WHERE f.user2 = :user_id AND NOT(f.type=-1 AND f.ignore_type=1)";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':user_id', $user_id, PDO::PARAM_INT);
    
        if ($stmt->execute()) {
            $r1 = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }

        // Get friend I send request to
        $sql = "SELECT f.*, u.user_name, u.user_id, u.avatar, u.status, u.state
        	FROM user_caches u
            LEFT JOIN friends f ON f.user2 = u.user_id
            WHERE f.user1 = :user_id AND NOT(f.type=-1 AND f.ignore_type=2)";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':user_id', $user_id, PDO::PARAM_INT);

        if ($stmt->execute()) {
            $r2 = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }

        return array_merge ($r1, $r2);
    }
    
    //=========================
    // BELLOW FOR ROOM QUERY
    //=========================
    public function getUserInRoom($room_id) {
        $r = array();

        $sql = "SELECT ru.*, u.*
        	FROM room_users ru
            LEFT JOIN user_caches u ON ru.user_id = u.user_id 
        	WHERE u.is_active = 1
            AND ru.room_id = :room_id";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':room_id', $room_id, PDO::PARAM_INT);
    
        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
		
		return $r;
	}
	
	public function updateUserRoom($room_id, $user_id, $user_lan_ip) {
        $stmt = $this->core->dbh->prepare("UPDATE room_users SET user_lan_ip = :user_lan_ip WHERE user_id = :user_id AND room_id = :room_id");
		$stmt->bindParam(':user_id', $user_id, PDO::PARAM_INT);
		$stmt->bindParam(':room_id', $room_id, PDO::PARAM_INT);
		$stmt->bindParam(':user_lan_ip', $user_lan_ip, PDO::PARAM_STR);
		
        if ($stmt->execute()) {
            return true;
        } else {
            return false;
        }
    }

	public function createRoomUser($data) {
        $sql = "INSERT INTO room_users (user_id, user_name, user_lan_ip, room_id)
                VALUES (:user_id, :user_name, :user_lan_ip, :room_id)";
        $stmt = $this->core->dbh->prepare($sql);
        if ($stmt->execute($data)) {
            return 1;
        } else {
            return 0;
        }
    }

    public function currentRoom($user_id) {
        $r = array();
        $sql = "SELECT ru.*, r.*
        	FROM room_users ru, rooms r 
        	WHERE r.room_id = ru.room_id
            AND ru.user_id = :user_id";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':user_id', $user_id, PDO::PARAM_INT);

        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
            if(!empty($r)) {
                return $r[0];
            }
        }

		return $r;
    }

	public function deleteRoomUser($room_id, $user_id) {
		$result = 0;
        $stmt = $this->core->dbh->prepare("DELETE FROM room_users WHERE room_id = :room_id AND user_id = :user_id");
        $stmt->bindParam(':user_id', $user_id, PDO::PARAM_INT);
        $stmt->bindParam(':room_id', $room_id, PDO::PARAM_INT);

        if ($stmt->execute()) {
            return 1;
        }

		return $result;
    }
    
    //=========================
    // BELLOW FOR MESSAGE QUERY
    //=========================
	public function oldPrivateMessage($user_id, $is_read, $date, $limit) {
        $r = array();
		
		$condition = "";
		$param = array();
		if ($user_id) {
			$condition .= " AND (m.user_id = :user_id OR m.receive_id = :user_id) AND m.user_id <> 0 AND m.receive_id <> 0 ";
			$param[':user_id'] = $user_id;
		}
		
		$param[':date'] = $date ? $date : time();
		$param[':is_read'] = $is_read;
		
		$sql = "SELECT message_id, user_id, receive_id, user_name, receive_name, message, notify, message_format, UNIX_TIMESTAMP(create_time) AS create_time
			FROM messages m
			WHERE m.is_active = 1
			{$condition}
			AND UNIX_TIMESTAMP(create_time) < :date
			AND is_read = :is_read
			ORDER BY create_time DESC
			LIMIT $limit";

		$stmt = $this->core->dbh->prepare($sql);
		
        if ($stmt->execute($param)) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
        return $r;
    }
	
	public function newPrivateMessage($user_id, $is_read, $date, $limit) {
        $r = array();
		
		$condition = "";
		$param = array();
		if ($user_id) {
			$condition .= " AND (m.user_id = :user_id OR m.receive_id = :user_id) AND m.user_id <> 0 AND m.receive_id <> 0 ";
			$param[':user_id'] = $user_id;
		}
		
		$param[':date'] = $date ? $date : time();
		$param[':is_read'] = $is_read;
		
		$sql = "SELECT message_id, user_id, receive_id, user_name, receive_name, message, notify, message_format, UNIX_TIMESTAMP(create_time) AS create_time
			FROM messages m
			WHERE m.is_active = 1
			{$condition}
			AND UNIX_TIMESTAMP(m.create_time) > :date
			AND m.is_read = :is_read
			ORDER BY create_time DESC
			LIMIT $limit";
		$stmt = $this->core->dbh->prepare($sql);
		
        if ($stmt->execute($param)) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
        return $r;
    }
    
    public function receiveAllMessage($user_id, $read_ids) {
        $r = array();

        $condition = "";
        $param = array();
        if ($read_ids) {
            $condition .= " AND m.message_id NOT IN ($read_ids) ";
        }

        $sql = "SELECT message_id, user_id, receive_id, user_name, receive_name, message, notify, UNIX_TIMESTAMP(create_time) AS create_time
        FROM messages m
        WHERE m.is_read = 0
            AND m.user_id <> 0
            AND m.receive_id <> 0
            AND m.receive_id = :receive_id
            $condition
        ORDER BY m.create_time DESC";
        $param[':receive_id'] = $user_id;
        $stmt = $this->core->dbh->prepare($sql);
    
        if ($stmt->execute($param)) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }

        // Update is_read = 1;
        if ($read_ids) {
            $stmt = $this->core->dbh->prepare("UPDATE messages SET is_read=1 WHERE message_id IN ($read_ids) ");
            $stmt->execute();
        }

        return $r;
    }

    public function historyMessage($user_id, $receive_id, $last_view_id, $limit) {
        $r = array();

        $condition = "";
        $param = array();
        if ($last_view_id) {
            $condition .= " AND m.message_id < :last_view_id ";
            $param[':last_view_id'] = $last_view_id;
        }
        
        $sql = "SELECT message_id, user_id, receive_id, user_name, receive_name, message, notify, UNIX_TIMESTAMP(create_time) AS create_time
        FROM messages m
        WHERE m.user_id = :user_id
            AND m.receive_id = :receive_id
            $condition
        ORDER BY m.create_time DESC
        LIMIT $limit";
        $param[':user_id'] = $user_id;
        $param[':receive_id'] = $receive_id;
        
        $stmt = $this->core->dbh->prepare($sql);
    
        if ($stmt->execute($param)) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
        return $r;
    }
}