var $api = axios.create({
  baseURL: utils.getQueryString('apiUrl') + '/' + utils.getQueryString('pluginId') + '/pages/test/',
  withCredentials: true
});

var data = {
  pageLoad: false,
  pageAlert: null,
  address: null,
  displayName: null,
  title: null,
  body: null
};

var methods = {
  submit: function () {
    var $this = this;

    utils.loading(true);
    $api.post('', {
      address: $this.address,
      displayName: $this.displayName,
      title: $this.title,
      body: $this.body
    }).then(function (response) {
      var res = response.data;

      swal2({
        toast: true,
        type: 'success',
        title: "测试邮件发送成功",
        showConfirmButton: false,
        timer: 2000
      });
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      utils.loading(false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.pageAlert = null;

    this.$validator.validate().then(function (result) {
      if (result) {
        $this.submit();
      }
    });
  },

  btnNavClick: function (pageName) {
    location.href = utils.getPageUrl(pageName);
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.pageLoad = true;
  }
});
