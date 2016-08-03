<?php

namespace v1\controllers;

class Sample extends \SlimController\SlimController
{

    public function indexAction()
    {

        /**
         * Access \SlimController\Slim $app
         */

        $this->app->response()->status(404);


        /**
         * Params
         */

        // reads "?data[foo]=some+value"
        $foo = $this->param('foo');

        // reads "data[bar][name]=some+value" only if POST!
        $bar = $this->param('bar.name', 'post');

        // all params of bar ("object attributes")
        //  "?data[bar][name]=me&data[bar][mood]=happy" only if POST!
        $bar = $this->param('bar');
        //error_log($bar['name']. ' is '. $bar['mood']);

        // reads multiple params in array
        $params = $this->params(array('foo', 'bar.name1', 'bar.name1'));
        //error_log($params['bar.name1']);

        // reads multiple params only if they are POST
        $params = $this->params(array('foo', 'bar.name1', 'bar.name1'), 'post');

        // reads multiple params only if they are POST and all are given!
        $params = $this->params(array('foo', 'bar.name1', 'bar.name1'), 'post', true);
        if (!$params) {
            error_log("Not all params given.. maybe some. Don't care");
        }

        // reads multiple params only if they are POST and replaces non given with defaults!
        $params = $this->params(array('foo', 'bar.name1', 'bar.name1'), 'post', array(
            'foo' => 'Some Default'
        ));


        /**
         * Redirect shortcut
         */

        if (false) {
            $this->redirect('/somewhere');
        }


        /**
         * Rendering
         */

        $this->render('folder/file', array(
            'foo' => 'bar'
        ));

    }
}
?>