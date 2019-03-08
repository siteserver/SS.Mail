var $url = '/pages/settings';
var $apiUrl = utils.getQueryString('apiUrl');

var data = {
  pageLoad: false,
  pageAlert: null,
  configInfo: null
};

var methods = {
  load: function () {
    var $this = this;

    $api.get($url).then(function (response) {
      var res = response.data;

      $this.configInfo = res.value;
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  submit: function () {
    var $this = this;

    utils.loading(true);
    $api.post($url, $this.configInfo).then(function (response) {
      var res = response.data;

      swal2({
        toast: true,
        type: 'success',
        title: "设置保存成功",
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
    location.href = pageName + '?apiUrl=' + encodeURIComponent($apiUrl);
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.load();
  }
});
