<?php

namespace DbHandler;

// Configuration Class
class Config {
    static $confArray = array(
        'db.user' => 'root',
        'db.password' => '',
        'db.host' => 'localhost',
        'db.basename' => 'task_manager',
        'db.port' => 3306
    );

    public static function read($name) {
        return self::$confArray[$name];
    }

    public static function write($name, $value) {
        self::$confArray[$name] = $value;
    }
}