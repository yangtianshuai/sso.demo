$PBExportHeader$w_main.srw
forward
global type w_main from window
end type
type cb_2 from commandbutton within w_main
end type
type cb_1 from commandbutton within w_main
end type
type log from multilineedit within w_main
end type
end forward

global type w_main from window
integer width = 2866
integer height = 1808
boolean titlebar = true
string title = "Untitled"
boolean controlmenu = true
boolean minbox = true
boolean maxbox = true
boolean resizable = true
long backcolor = 67108864
string icon = "AppIcon!"
boolean center = true
cb_2 cb_2
cb_1 cb_1
log log
end type
global w_main w_main

type variables

end variables

event open;string app_id = "6ea54b9377934f08b4648e7a00c6c394"  //应用ID(来自运维平台)
string secret = "b6ede2dbdd9a40a9a0f6feb14d56ec99"  //应用密钥(来自运维平台)
string user_type = "GH"  //账户类别：OA（内网OA）、GH（工号）、DSF（第三方）
string user_name = "0745"  //登录用户名
string password = "0745"  //登录密码

this.log.text=""

if gs_sso.sso_connect(app_id, secret) then
	if gs_access_token="" then
		//通过登录获取SSO令牌
		gs_access_token = gs_sso.get_token(user_type, user_name, password)
	else
		gs_access_token = gs_sso.get_token('','','')
	end if
else	
	this.log.text+="连接失败："+gs_sso.error() + "~n~r"	
	gs_access_token=""
end if

if gs_access_token="" then
	this.log.text+="获取AcessToken失败："+gs_sso.error() + "~n~r"
	//可能由于本地插件未注册，导致无法获取AccessToken
	
	//不再使用单点登录
	
	return
else	
	this.log.text+="获取AcessToken："+gs_access_token + "~n~r"
end if

//获取SSO用户
gs_user = gs_sso.get_user(gs_access_token)

if isnull(gs_user) then
	this.log.text+="获取用户信息失败："+gs_sso.error() + "~n~r"
	return
else
	this.log.text+="用户ID："+gs_user.id + "~n~r"
	this.log.text+="用户姓名："+gs_user.name + "~n~r"
	this.log.text+="登录账号："+gs_user.login_name + "~n~r"
	this.log.text+="员工ID："+gs_user.employee_id + "~n~r"
	this.log.text+="工号："+gs_user.user_name + "~n~r"
	this.log.text+="Job："+gs_user.job + "~n~r"
	this.log.text+="Title："+gs_user.title + "~n~r"
end if

messagebox("",this.log.text)

end event

on w_main.create
this.cb_2=create cb_2
this.cb_1=create cb_1
this.log=create log
this.Control[]={this.cb_2,&
this.cb_1,&
this.log}
end on

on w_main.destroy
destroy(this.cb_2)
destroy(this.cb_1)
destroy(this.log)
end on

event timer;if gs_access_token="" then
	//检测到access_token失效，需要退出登录
	return
end if

//建议间隔60秒
if gs_sso.heat_beat(gs_access_token) then
	//登录状态正常
	gs_access_token=gs_sso.token()
else
	gs_access_token=""
	//messagebox("",gs_sso.error())
end if

if gs_access_token="" then
	gs_access_token = gs_sso.get_token('','','')
end if
end event

type cb_2 from commandbutton within w_main
integer x = 1358
integer y = 1436
integer width = 457
integer height = 128
integer taborder = 30
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Arial"
string text = "退出"
end type

type cb_1 from commandbutton within w_main
integer x = 649
integer y = 1436
integer width = 457
integer height = 128
integer taborder = 20
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Arial"
string text = "登录"
end type

type log from multilineedit within w_main
integer x = 293
integer y = 188
integer width = 2043
integer height = 1112
integer taborder = 10
integer textsize = -12
integer weight = 400
fontcharset fontcharset = ansi!
fontpitch fontpitch = variable!
fontfamily fontfamily = swiss!
string facename = "Arial"
long textcolor = 33554432
string text = "none"
borderstyle borderstyle = stylelowered!
end type

