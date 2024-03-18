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

event open;string app_id = "6ea54b9377934f08b4648e7a00c6c394"  //Ӧ��ID(������άƽ̨)
string secret = "b6ede2dbdd9a40a9a0f6feb14d56ec99"  //Ӧ����Կ(������άƽ̨)
string user_type = "GH"  //�˻����OA������OA����GH�����ţ���DSF����������
string user_name = "0745"  //��¼�û���
string password = "0745"  //��¼����

this.log.text=""

if gs_sso.sso_connect(app_id, secret) then
	if gs_access_token="" then
		//ͨ����¼��ȡSSO����
		gs_access_token = gs_sso.get_token(user_type, user_name, password)
	else
		gs_access_token = gs_sso.get_token('','','')
	end if
else	
	this.log.text+="����ʧ�ܣ�"+gs_sso.error() + "~n~r"	
	gs_access_token=""
end if

if gs_access_token="" then
	this.log.text+="��ȡAcessTokenʧ�ܣ�"+gs_sso.error() + "~n~r"
	//�������ڱ��ز��δע�ᣬ�����޷���ȡAccessToken
	
	//����ʹ�õ����¼
	
	return
else	
	this.log.text+="��ȡAcessToken��"+gs_access_token + "~n~r"
end if

//��ȡSSO�û�
gs_user = gs_sso.get_user(gs_access_token)

if isnull(gs_user) then
	this.log.text+="��ȡ�û���Ϣʧ�ܣ�"+gs_sso.error() + "~n~r"
	return
else
	this.log.text+="�û�ID��"+gs_user.id + "~n~r"
	this.log.text+="�û�������"+gs_user.name + "~n~r"
	this.log.text+="��¼�˺ţ�"+gs_user.login_name + "~n~r"
	this.log.text+="Ա��ID��"+gs_user.employee_id + "~n~r"
	this.log.text+="���ţ�"+gs_user.user_name + "~n~r"
	this.log.text+="Job��"+gs_user.job + "~n~r"
	this.log.text+="Title��"+gs_user.title + "~n~r"
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
	//��⵽access_tokenʧЧ����Ҫ�˳���¼
	return
end if

//������60��
if gs_sso.heat_beat(gs_access_token) then
	//��¼״̬����
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
string text = "�˳�"
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
string text = "��¼"
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

