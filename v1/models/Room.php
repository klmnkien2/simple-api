<?php

/**
 * This is an example of User Class using PDO
 *
 */

namespace models;
use DbHandler\Core;
use PDO;

class Room {

    protected $core;

    function __construct() {
        $this->core = Core::getInstance();
        //$this->core->dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    }

    public function getRoomList() {
        $r = array();

        $sql = "SELECT r.room_id, r.room_name, r.create_user, r.host_ip, s.server_id, s.hub
        	FROM rooms r
			LEFT JOIN servers s ON r.server_id = s.server_id
        	WHERE s.is_active = 1 AND r.is_active = 1";
        $stmt = $this->core->dbh->prepare($sql);

        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        } else {
            $r = 0;
        }
        return $r;
    }

    public function createRoom($data) {
        $this->core->dbh->setAttribute(PDO::ATTR_EMULATE_PREPARES,TRUE);
        $sql = "INSERT INTO rooms (room_name, server_id, host_ip, create_user)
                VALUES (:room_name, :server_id, :host_ip, :create_user)";
        $stmt = $this->core->dbh->prepare($sql);
        if ($stmt->execute($data)) {
            return $this->core->dbh->lastInsertId();
        } else {
            return null;
        }
    }

    public function deleteRoom($id) {
        $stmt = $this->core->dbh->prepare("DELETE FROM rooms WHERE room_id = :id");
        $stmt->bindParam(':id', $id, PDO::PARAM_INT);

        if ($stmt->execute()) {
            return true;
        } else {
            return false;
        }
    }

    public function getRoomById($room_id) {
        $r = null;

        $sql = "SELECT r.room_id, r.room_name, r.create_user, r.host_ip, s.server_id, s.hub
        	FROM rooms r
			LEFT JOIN servers s ON r.server_id = s.server_id
        	WHERE s.is_active = 1 AND r.is_active = 1 AND r.room_id=:room_id";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':room_id', $room_id, PDO::PARAM_INT);

        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
        if (is_array($r) && !empty($r)) {
            $r = $r[0];
        }

        return $r;
    }

    //=========================
    // BELLOW FOR MESSAGE QUERY
    //=========================

    public function createMessage($message) {
        try {
            $sql = "INSERT INTO messages (user_id, user_name, receive_id, receive_name, room_id, notify, message)
                    VALUES (:user_id, :user_name, :receive_id, :receive_name, :room_id, :notify, :message)";
            $stmt = $this->core->dbh->prepare($sql);
            if ($stmt->execute($message)) {
                return $this->core->dbh->lastInsertId();
            } else {
                return null;
            }
        } catch(PDOException $e) {
            echo $e->getMessage();
            return null;
        }
    }

    public function oldMessageByRoom($room_id, $message_id, $limit) {
        $r = array();
	
		if ($message_id) {
			$sql = "SELECT *
				FROM messages m
				WHERE 
				m.receive_id = 0
				AND m.room_id = :room_id
				AND message_id < :message_id
				ORDER BY create_time DESC
				LIMIT $limit";
			$stmt = $this->core->dbh->prepare($sql);
			$stmt->bindParam(':room_id', $room_id, PDO::PARAM_INT);
			$stmt->bindParam(':message_id', $message_id, PDO::PARAM_INT);
		} else {
			$sql = "SELECT *
				FROM messages m
				WHERE m.receive_id = 0
				AND m.room_id = :room_id
				ORDER BY create_time DESC
				LIMIT $limit";
			$stmt = $this->core->dbh->prepare($sql);
			$stmt->bindParam(':room_id', $room_id, PDO::PARAM_INT);
		}
        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
        return $r;
    }
	
	public function newMessageByRoom($room_id, $message_id) {
        $r = array();
	
		$sql = "SELECT *
			FROM messages m
			WHERE m.receive_id = 0
				AND m.room_id = :room_id
			AND message_id > :message_id
			ORDER BY create_time DESC";
		$stmt = $this->core->dbh->prepare($sql);
		$stmt->bindParam(':room_id', $room_id, PDO::PARAM_INT);
		$stmt->bindParam(':message_id', $message_id, PDO::PARAM_INT);
		
        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
        return $r;
    }
}