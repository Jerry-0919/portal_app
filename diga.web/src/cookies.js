function deleteCookie(name) {
  document.cookie =
    name + "=; Path=/platform; Expires=Thu, 01 Jan 1970 00:00:01 GMT;"
}
function getCookie(name) {
  var pattern = RegExp(name + "=.[^;]*")
  var matched = document.cookie.match(pattern)
  if (matched) {
    var cookie = matched[0].split("=")
    return cookie[1]
  }
  return false
}
function setCookie(c_name, value, exdays) {
  var exdate = new Date();
  exdate.setDate(exdate.getDate() + exdays)
  var c_value =
    escape(value) + (exdays == null ? "" : "; expires=" + exdate.toUTCString())
  document.cookie = c_name + "=" + c_value
}
export default {
  deleteCookie,
  getCookie,
  setCookie,
};
