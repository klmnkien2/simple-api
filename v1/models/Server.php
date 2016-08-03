<?php

/**
 * This is an example of User Class using PDO
 *
 */

namespace models;
use DbHandler\Core;
use PDO;

class Server {

    protected $core;

    function __construct() {
        $this->core = Core::getInstance();
        //$this->core->dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    }
    
    // Get all users
    public function getRandom() {
        $r = array();

        $sql = "SELECT * FROM servers WHERE number_connected = 0 LIMIT 1";
        $stmt = $this->core->dbh->prepare($sql);

        if ($stmt->execute()) {
            $r = $stmt->fetchAll(PDO::FETCH_ASSOC);
        }
		
        if(!empty($r)) {			
			$notify = $this->notifyUsing($r[0]['server_id']);
			return $r[0];
        } else {
			return null;
		}
    }
	
	public function notifyUsing($server_id) {
		$stmt = $this->core->dbh->prepare("UPDATE servers SET number_connected = 1 WHERE server_id = :server_id");
        $stmt->bindParam(':server_id', $server_id, PDO::PARAM_INT);

        if ($stmt->execute()) {
            return true;
        } else {
            return false;
        }
	}

	public function unbindServer($room_id) {
	    $stmt = $this->core->dbh->prepare("UPDATE servers SET number_connected = 0 WHERE server_id in (SELECT server_id FROM rooms WHERE room_id = :room_id)");
	    $stmt->bindParam(':room_id', $room_id, PDO::PARAM_INT);

	    if ($stmt->execute()) {
	        return true;
	    } else {
	        return false;
	    }
	}

}