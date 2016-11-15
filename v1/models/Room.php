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

    public function getAllTree($root_id) {
        $nodes = $this->listByParent($root_id);
        foreach ($nodes as &$node) {
            $child = $this->getAllTree($node['room_id']);
            if (!empty($child)) {
                $node['child'] = $child;
            }
        }
        return $nodes;
    }

    public function listByParent($parent_id) {
        $r = array();
    
        $sql = "SELECT r.*, s.host, s.port, s.hub
            FROM rooms r
            LEFT JOIN servers s ON s.server_id = r.server_id
            WHERE parent_id=?";
        $stmt = $this->core->dbh->prepare($sql);
    
        if ($stmt->execute(array($parent_id))) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        } else {
            $r = 0;
        }
        return $r;
    }
    
    public function getRoomList() {
        $r = array();

        $sql = "SELECT r.*, s.host, s.port, s.hub
            FROM rooms r
            LEFT JOIN servers s ON s.server_id = r.server_id";
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

    public function syncRoom(&$data) {

        $sql = "SELECT r.room_id, r.room_name, r.image, r.members, r.maximum, r.server_id, s.host, s.port, s.hub
            FROM rooms r
            LEFT JOIN servers s ON s.server_id = r.server_id
            WHERE r.room_id=:room_id";
        $stmt = $this->core->dbh->prepare($sql);
        $stmt->bindParam(':room_id', $data['id'], PDO::PARAM_INT);

        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
    
        if (!empty($r)) {
            $r = $r[0];
            $data['room_id'] = $r['room_id'];
            $data['room_name'] = $data['name']; // room Name nen lay tu api
            //$data['image'] = $r['image']; Image nen lay tu api
            $data['members'] = $r['members'];
            $data['maximum'] = $r['maximum'];
            $data['server_id'] = $r['server_id'];
            $data['host'] = $r['host'];
            $data['port'] = $r['port'];
            $data['hub'] = $r['hub'];
        } else {
            //Insert
            $data['room_id'] = $data['id'];
            $data['room_name'] = $data['name'];
            $data['image'] = !empty($data['image']) ? $data['image'] : "";
            $data['members'] = 0;
            $data['maximum'] = 100;
            if ($data['has_child'] == 0) {
                $server = $this->getFreeServer();
                $data['server_id'] = $server['server_id'];
                $data['host'] = $server['host'];
                $data['port'] = $server['port'];
                $data['hub'] = $server['hub'];
            } else {
                $data['server_id'] = -1;
            }
            
            $stmt = $this->core->dbh->prepare("INSERT INTO rooms (room_id, room_name, image, members, maximum, parent_id, has_child, server_id) " .
                "VALUES (:room_id, :room_name, :image, :members, :maximum, :parent_id, :has_child, :server_id)");
            $stmt->bindParam(':room_id', $data['room_id'], PDO::PARAM_INT);
            $stmt->bindParam(':room_name', $data['room_name'], PDO::PARAM_STR);
            $stmt->bindParam(':image', $data['image'], PDO::PARAM_STR);
            $stmt->bindParam(':parent_id', $data['parent_id'], PDO::PARAM_INT);
            $stmt->bindParam(':has_child', $data['has_child'], PDO::PARAM_INT);
            $stmt->bindParam(':server_id', $data['server_id'], PDO::PARAM_INT);
            $stmt->bindParam(':members', $data['members'], PDO::PARAM_INT);
            $stmt->bindParam(':maximum', $data['maximum'], PDO::PARAM_INT);
            $stmt->execute();
        }
    }
    
    public function getFreeServer() {
        $r = array();

        $sql = "SELECT server_id, host, port, hub
            FROM servers 
            WHERE server_id NOT IN (SELECT server_id FROM rooms r) LIMIT 1";
        $stmt = $this->core->dbh->prepare($sql);

        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
        
        if(!empty($r)) {    
            return $r[0];
        } else {
            return null;
        }
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
                return -1;
            }
        } catch(PDOException $e) {
            echo $e->getMessage();
            return -1;
        }
    }

    public function receiveNewMessage($room_id, $read_ids) {
        $r = array();

        $condition = "";
        $param = array();
        if ($read_ids) {
            $condition .= " AND m.message_id NOT IN ($read_ids) ";
        }

        $sql = "SELECT message_id, user_id, user_name, message, notify, UNIX_TIMESTAMP(create_time) AS create_time
        FROM messages m
        WHERE m.is_read = 0
            AND m.user_id <> 0
            AND m.room_id = :room_id
            $condition
        ORDER BY m.create_time DESC";
        $param[':room_id'] = $room_id;
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

    public function historyMessage($room_id, $last_view_id, $limit) {
        $r = array();

        $condition = "m.room_id = :room_id";
        $param = array();
        $param[':room_id'] = $room_id;
        if ($last_view_id) {
            $condition .= " AND m.message_id < :last_view_id ";
            $param[':last_view_id'] = $last_view_id;
        }
        
        $sql = "SELECT message_id, user_id, user_name, room_id, message, notify, UNIX_TIMESTAMP(create_time) AS create_time
        FROM messages m
        WHERE $condition
        ORDER BY m.create_time DESC
        LIMIT $limit";

        $stmt = $this->core->dbh->prepare($sql);
    
        if ($stmt->execute($param)) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
        return $r;
    }
}