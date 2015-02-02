<?php /* Smarty version Smarty-3.1.12, created on 2013-09-24 23:34:27
         compiled from "templates/login.tpl" */ ?>
<?php /*%%SmartyHeaderCode:137869033351f608b71982f2-00019420%%*/if(!defined('SMARTY_DIR')) exit('no direct access allowed');
$_valid = $_smarty_tpl->decodeProperties(array (
  'file_dependency' => 
  array (
    '81a0270564c79ee7a1c4f40d2a5e8bcfac7e3570' => 
    array (
      0 => 'templates/login.tpl',
      1 => 1380079752,
      2 => 'file',
    ),
  ),
  'nocache_hash' => '137869033351f608b71982f2-00019420',
  'function' => 
  array (
  ),
  'version' => 'Smarty-3.1.12',
  'unifunc' => 'content_51f608b71dbde2_38824061',
  'variables' => 
  array (
    'info' => 0,
  ),
  'has_nocache_code' => false,
),false); /*/%%SmartyHeaderCode%%*/?>
<?php if ($_valid && !is_callable('content_51f608b71dbde2_38824061')) {function content_51f608b71dbde2_38824061($_smarty_tpl) {?><?php echo $_smarty_tpl->getSubTemplate ("templates/header.tpl", $_smarty_tpl->cache_id, $_smarty_tpl->compile_id, null, null, array('title'=>"Gate - Login"), 0);?>


<?php if (isset($_smarty_tpl->tpl_vars['info']->value)){?>
<div id='info_box'><?php echo $_smarty_tpl->tpl_vars['info']->value;?>
</div>
<?php }?>

<form action='index.php' method='post'>

<div id='login_box'>
  <div id='login_name_label'>Name</div>
  <div id='login_name'><input name='login_name' type='text'/></div>
  <div id='login_pw_label'>Password</div>
  <div id='login_pw'><input type='password' name='login_pw' /></div>
  <div id='login_button'><input type='submit' /></div>
</div>

</form>

<?php echo $_smarty_tpl->getSubTemplate ("templates/footer.tpl", $_smarty_tpl->cache_id, $_smarty_tpl->compile_id, null, null, array(), 0);?>

<?php }} ?>