<?php
use v1\controllers\Home;
// define a working directory
define('APP_PATH', dirname(__DIR__)); // PHP v5.3+

// load slim
require APP_PATH . '/libs/Slim/Slim.php';
\Slim\Slim::registerAutoloader();
// load slim controller
require APP_PATH . '/libs/SlimController/Slim.php';

function autoLoad($dir) {
    $files = glob($dir . '/*.php');
    foreach ($files as $file) {
        require($file);
    }
}

autoLoad(APP_PATH . '/libs/database/');
autoLoad(APP_PATH . '/v1/controllers/');
autoLoad(APP_PATH . '/v1/models');

// init app
$app = New \SlimController\Slim(array(
    'templates.path'             => APP_PATH . '/templates',
    'controller.class_prefix'    => '\\v1\\controllers',
    'controller.param_prefix'    => '', // VERY IMPORTANT, FUCKING DEFAULT IS 'data.'
    'controller.method_suffix'   => 'Action',
    'controller.template_suffix' => 'php'
));

$app->addRoutes(array(
    // BELOW PART FOR [ROOM] CONTROLLER ROUTER
    '/room/list/' => 'Room:list',
    '/room/create/' => array('post' => array('Room:create', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
    '/room/delete/' => array('delete' => array('Room:delete', function() {
            error_log("THIS ROUTE IS ONLY DELETE");
        }
    )),
    '/room/message/' => array('post' => array('Room:message', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
    '/room/invite/' => array('post' => array('Room:invite', function() {
    error_log("THIS ROUTE IS ONLY POST");
    }
    )),
    '/room/history/' => 'Room:history',
    '/room/show/' => 'Room:show',
    // BELOW PART FOR [USER] CONTROLLER ROUTER
    '/friend/list/' => 'User:friendList',
	'/friend/add/' => array('post' => array('User:addFriend', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
	'/friend/delete/' => array('post' => array('User:deleteFriend', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
	'/friend/accept/' => array('post' => array('User:acceptFriend', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
	'/friend/deny/' => array('post' => array('User:denyFriend', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
	'/user/show/' => 'User:show',
	'/user/room/' => 'User:room',
	'/user/history/' => 'User:history',
	'/user/join/' => array('post' => array('User:join', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
	'/user/leave/' => array('post' => array('User:leave', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
	'/user/login/'     => array('post' => array('User:login', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
	'/user/logout/'     => array('post' => array('User:logout', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
    )),
), function() {
    error_log("APPENDED MIDDLEWARE FOR ALL ROUTES. MAY BE AUTHEN FUNCTION?");
});

/* SAMPLE OF ROUTE - how to integrate the Slim middleware
$app->addRoutes(array(
    '/' => array('Home:index', function() {
        error_log("MIDDLEWARE FOR SINGLE ROUTE");
    },
    function() {
        error_log("ADDITIONAL MIDDLEWARE FOR SINGLE ROUTE");
    }
        ),
        '/hello/:name' => array('post' => array('Home:hello', function() {
            error_log("THIS ROUTE IS ONLY POST");
        }
            ))
), function() {
    error_log("APPENDED MIDDLEWARE FOR ALL ROUTES");
});
*/
$app->run();
?>