<?php
function encode_ipad_video($in_filename, $out_filename) {
        print($in_filename." -> ".$out_filename."\n");
        shell_exec("/usr/bin/nice -n 15 /usr/bin/avconv -y -i ".$in_filename." -codec:v libx264 -pre slow -profile:v baseline -level 30 -maxrate 500k -bufsize 10000k -b:v 500k -f mp4 -vf scale=-1:540 ".$out_filename." </dev/null");
        shell_exec("chown knobby:users ".$out_filename);
        print("done encoding ".$out_filename."\n");
}

function encode_webm_video($in_filename, $out_filename) {
        print($in_filename." -> ".$out_filename."\n");
        shell_exec("/usr/bin/nice -n 15 /usr/bin/avconv -y -i ".$in_filename." -codec:v libvpx -cpu-used 0 -b:v 500k -qmin 10 -qmax 42 -maxrate 500k -bufsize 10000k -vf scale=-1:540 ".$out_filename." </dev/null");
        shell_exec("chown knobby:users ".$out_filename);
        print("done encoding ".$out_filename."\n");
}

function generate_image($in_filename, $out_filename) {
        print($in_filename." -> ".$out_filename."\n");
        shell_exec("/usr/bin/convert ".$in_filename." -resize 160x94 ".$out_filename);
        shell_exec("chmod 644 ".$in_filename." ".$out_filename);
}
print "starting up...";

$worker = new GearmanWorker();
if (!$worker->addServer("gearmand", "4730")) {
 die("Unable to add server!");
}
$worker->addFunction("encode_ipad_video", function($job) { $work = json_decode($job->workload(), true); return encode_ipad_video($work['in'], $work['out']); });
$worker->addFunction("encode_webm_video", function($job) { $work = json_decode($job->workload(), true); return encode_webm_video($work['in'], $work['out']); });
$worker->addFunction("generate_image", function($job) { $work = json_decode($job->workload(), true); return generate_image($work['in'], $work['out']); });

$worker->setTimeout(60000);

echo "waiting for work...";
try {
while ($worker->work() || GEARMAN_TIMEOUT == $worker->returnCode()) {
    if (GEARMAN_TIMEOUT == $worker->returnCode()) {
        $echo = @$worker->echo(1);
        if (!$echo) {
            echo 'Failed to connect to Gearman Gerver.'. PHP_EOL;
            break;
        }
    } elseif (GEARMAN_SUCCESS != $worker->returnCode()) {
        echo 'error!'. PHP_EOL;
        break;
    }
}
} catch (GearmanException $e) {
  die("Exception caused!\n");
}
?>
