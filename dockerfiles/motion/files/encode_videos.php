<?php

function replace_extension($filename, $new_extension) {
    $info = pathinfo($filename);
    return $info['dirname'] . '/' . $info['filename'] . '.' . $new_extension;
}

$filename = $argv[1];
$ipad_filename = replace_extension($filename, 'ipad.mp4');
$webm_filename = replace_extension($filename, 'webm');

$client = new GearmanClient();
$client->addServer("gearmand");

$client->doBackground("encode_ipad_video", json_encode(array('in' => $filename, 'out' => $ipad_filename)));
$client->doBackground("encode_webm_video", json_encode(array('in' => $filename, 'out' => $webm_filename)));

?>
