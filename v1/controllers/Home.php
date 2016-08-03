<?php

namespace v1\controllers;

class Home extends \SlimController\SlimController
{

    public function indexAction()
    {
        $this->render('home/index', array(
            'someVar' => date('c')
        ));
    }

    public function serverAction()
    {
        // Sample get database data
        $oServer = new \models\Server();
        $server = $oServer->getRandom();
        var_dump($server);
    }
}
?>