//OAuth2授权码模式

//sso接口
/*ops传入参数包括：
  参数1：appId（应用ID）
  参数2：baseUrl（SSO服务器的请求HTTP地址）
  参数3：redirectUrl（SSO服务器请求回调应用的HTTP地址）
  参数4：logoutUrl（应用的登出HTTP地址，应用后端地址用于清除Session）
*/
var SSO = function (ops) {
  this.error = '';
  this.config = {
    api: function (api) {
      return ops.baseUrl + '/' + api;
    },
  };
  //发起请求
  this.authorize = function (request) {
    if (!request) {
      this.error = '请求参数不能为空';
      return;
    }
    if (!request.state) {
      this.error = 'state必填';
      return;
    }
    if (!request.app_id) {
      request.app_id = ops.appId;
    }
    if (!request.redirect_uri) {
      request.redirect_uri = ops.redirectUrl;
    }
    if (!request.response_type) {
      request.response_type = 'code';
    }
    var url = this.config.api('oauth2/authorize');
    var param = '';
    for (var key in request) {
      if (param.length > 0) {
        param += '&';
      }
      param += key;
      param += '=';
      param += request[key];
    }
    url += '?' + param;
    window.location.href = url;
  };
  //获取Token
  this.token = function (code, callback) {
    var url = this.config.api('oauth2/token');
    url += '?grant_type=authorization_code&code=' + code;
    ajax.get(url, (result) => {
      if (callback) callback(result);
    });
  };
  //刷新Token
  this.refresh = function (refresh_token, callback) {
    var url = this.config.api('oauth2/token');
    url += '?grant_type=refresh_token&refresh_token=' + refresh_token;
    ajax.get(url, (result) => {
      if (callback) callback(result);
    });
  };
  //获取用户信息
  this.getUser = function (access_token, callback) {
    var url = this.config.api('sso/validate');
    url +=
      '?app_id=' +
      ops.appId +
      '&ticket=' +
      access_token +
      '&path=' +
      ops.logoutUrl;
    ajax.get(url, (result) => {
      if (callback) callback(result);
    });
  };
  this.logout = function (access_token, redirect_uri) {
    var url = this.config.api('sso/logout');
    url +=
      '?app_id=' +
      ops.appId +
      '&ticket=' +
      access_token +
      '&redirect_uri=' +
      redirect_uri;
    window.location.href = url;
  };
};

function beforeReturn(option) {
  if (option.xhr.readyState == 4) {
    var result = {};
    if (option.xhr.status == 200) {
      result = JSON.parse(option.xhr.responseText);
    } else {
      result.code = 0;
      if (option.xhr.responseText.length > 0) {
        result.error = option.xhr.responseText;
      } else {
        result.error = '调用失败';
      }
    }
    option.callback(result);
  }
}

//封装get和post请求
var ajax = {
  get: function (url, fn) {
    var xhr = new XMLHttpRequest();
    xhr.open('GET', url, true);
    xhr.onreadystatechange = function () {
      beforeReturn({
        xhr: xhr,
        callback: fn,
      });
    };
    if (xhr) {
      xhr.send();
    } else {
      fn.call(this, xhr.responseText);
    }
  },
  post: function (url, data, fn) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', url, true);
    xhr.onreadystatechange = function () {
      beforeReturn({
        xhr: xhr,
        callback: fn,
      });
    };
    xhr.send(JSON.stringify(data));
  },
};
