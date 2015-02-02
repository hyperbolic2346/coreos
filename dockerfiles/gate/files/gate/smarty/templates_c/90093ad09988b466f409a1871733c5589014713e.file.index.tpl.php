<?php /* Smarty version Smarty-3.1.12, created on 2014-03-23 19:33:13
         compiled from "templates/index.tpl" */ ?>
<?php /*%%SmartyHeaderCode:196219840351f608ba216a93-06995329%%*/if(!defined('SMARTY_DIR')) exit('no direct access allowed');
$_valid = $_smarty_tpl->decodeProperties(array (
  'file_dependency' => 
  array (
    '90093ad09988b466f409a1871733c5589014713e' => 
    array (
      0 => 'templates/index.tpl',
      1 => 1395617574,
      2 => 'file',
    ),
  ),
  'nocache_hash' => '196219840351f608ba216a93-06995329',
  'function' => 
  array (
  ),
  'version' => 'Smarty-3.1.12',
  'unifunc' => 'content_51f608ba2b4305_93149741',
  'variables' => 
  array (
    'info' => 0,
    'day' => 0,
    'live_cam' => 0,
    'access' => 0,
    'calendar' => 0,
    'camera_data' => 0,
    'ev' => 0,
    'add_user' => 0,
    'cur' => 0,
    'edit_user' => 0,
    'users' => 0,
    'user' => 0,
  ),
  'has_nocache_code' => false,
),false); /*/%%SmartyHeaderCode%%*/?>
<?php if ($_valid && !is_callable('content_51f608ba2b4305_93149741')) {function content_51f608ba2b4305_93149741($_smarty_tpl) {?><?php echo $_smarty_tpl->getSubTemplate ("templates/header.tpl", $_smarty_tpl->cache_id, $_smarty_tpl->compile_id, null, null, array('title'=>"Gate"), 0);?>


<link rel="stylesheet" type="text/css" href="templates/base.css" media="screen, handheld" />
<link rel="stylesheet" type="text/css" href="templates/enhanced.css" media="screen  and (min-width: 40.5em)" />
<!--[if (lt IE 9)&(!IEMobile)]>
<link rel="stylesheet" type="text/css" href="templates/enhanced.css" />
<![endif]-->

<script type="text/javascript" src="/lib/html5lightbox/html5lightbox.js"></script>
<script type="text/javascript" src="/js/index.js"></script>

<div id='info'><?php if (isset($_smarty_tpl->tpl_vars['info']->value)){?><?php echo $_smarty_tpl->tpl_vars['info']->value;?>
<?php }else{ ?>&nbsp;<?php }?></div>
<div id='day'><?php echo $_smarty_tpl->tpl_vars['day']->value;?>
</div>

<?php if (isset($_smarty_tpl->tpl_vars['live_cam']->value)){?>
<div id='live_video_toggle_div'></div>
<div id='live_video_div'>
	<div id='live_label'>Now</div>
	<div id='live_video'><img id='live_video_img' data-feed='<?php echo $_smarty_tpl->tpl_vars['live_cam']->value;?>
' /></div>
</div>

<?php if (isset($_smarty_tpl->tpl_vars['access']->value)&&isset($_smarty_tpl->tpl_vars['access']->value['control'])){?>
<div id='gate_control_div'>
</div>
<?php }?>
<?php }?>

<div id='calendar'>
<?php echo $_smarty_tpl->tpl_vars['calendar']->value;?>

</div>

<?php if (isset($_smarty_tpl->tpl_vars['live_cam']->value)&&isset($_smarty_tpl->tpl_vars['access']->value['control'])){?>
<br style="clear:both;"/>
<div id='camera_events_toggle_div'></div>
<?php }?>

<div id='camera_events'>
<?php if (isset($_smarty_tpl->tpl_vars['camera_data']->value)){?>
<?php  $_smarty_tpl->tpl_vars['ev'] = new Smarty_Variable; $_smarty_tpl->tpl_vars['ev']->_loop = false;
 $_from = $_smarty_tpl->tpl_vars['camera_data']->value; if (!is_array($_from) && !is_object($_from)) { settype($_from, 'array');}
 $_smarty_tpl->tpl_vars['ev']->index=-1;
foreach ($_from as $_smarty_tpl->tpl_vars['ev']->key => $_smarty_tpl->tpl_vars['ev']->value){
$_smarty_tpl->tpl_vars['ev']->_loop = true;
 $_smarty_tpl->tpl_vars['ev']->index++;
?>
	<div class='camera_event'>
		<?php if (isset($_smarty_tpl->tpl_vars['access']->value['delete'])){?>
		<div class='camera_delete' onClick='delete_entry(this, "<?php echo $_smarty_tpl->tpl_vars['ev']->key;?>
")'><img src='img/x.png' height="25" width="25" /></div>
		<?php }?>
		<div class='camera_time'><?php echo $_smarty_tpl->tpl_vars['ev']->value['pretty_time'];?>
</div>
		<div class='camera_video'>
			<div id='camera_video<?php echo $_smarty_tpl->tpl_vars['ev']->index;?>
'>
				<a class="html5lightbox" href="<?php echo $_smarty_tpl->tpl_vars['ev']->value['movie'];?>
.webm" data-ipad="<?php echo $_smarty_tpl->tpl_vars['ev']->value['movie'];?>
.ipad.mp4" data-iphone="<?php echo $_smarty_tpl->tpl_vars['ev']->value['movie'];?>
.ipad.mp4" data-width="640" data-height="480"><img <?php if (isset($_smarty_tpl->tpl_vars['ev']->value['refresh'])){?>class="refresh" <?php }?>data-refresh="<?php echo $_smarty_tpl->tpl_vars['ev']->value['refresh_id'];?>
" src="<?php echo $_smarty_tpl->tpl_vars['ev']->value['thumbnail'];?>
.thumb.jpg" width="100%" /></a>
			</div>
		</div>
	</div>
<?php } ?>
<?php }else{ ?>
No events
<?php }?>
</div>

<?php if (isset($_smarty_tpl->tpl_vars['add_user']->value)){?>
	<form name="adduser" action="<?php echo $_smarty_tpl->tpl_vars['cur']->value;?>
" method="post"><input type="text" name="new_username" /><input type="password" name="new_pw" /><input type="submit" /></form>
<?php }?>

<?php if (isset($_smarty_tpl->tpl_vars['edit_user']->value)){?>
	<form name="edit_user" action="<?php echo $_smarty_tpl->tpl_vars['cur']->value;?>
" method="post">
          <select name="edit_user_id">
<?php  $_smarty_tpl->tpl_vars['user'] = new Smarty_Variable; $_smarty_tpl->tpl_vars['user']->_loop = false;
 $_from = $_smarty_tpl->tpl_vars['users']->value; if (!is_array($_from) && !is_object($_from)) { settype($_from, 'array');}
foreach ($_from as $_smarty_tpl->tpl_vars['user']->key => $_smarty_tpl->tpl_vars['user']->value){
$_smarty_tpl->tpl_vars['user']->_loop = true;
?>
            <option value="<?php echo $_smarty_tpl->tpl_vars['user']->value['user_id'];?>
"><?php echo $_smarty_tpl->tpl_vars['user']->value['username'];?>
</option>
<?php } ?>
          </select>
          <input type="password" name="new_pw" /><input type="submit" /></form>
<?php }?>

<?php echo $_smarty_tpl->getSubTemplate ("templates/footer.tpl", $_smarty_tpl->cache_id, $_smarty_tpl->compile_id, null, null, array(), 0);?>

<?php }} ?>