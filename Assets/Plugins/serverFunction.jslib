mergeInto(LibraryManager.library, {

  GetData: function (url) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open( "GET", Pointer_stringify(url), false ); // false for synchronous request
    xmlHttp.send( null );
    var text = xmlHttp.responseText;
    var bufferSize = lengthBytesUTF8(text) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(text, buffer, bufferSize);
    return buffer;
  },
  
  getCookies: function () {
      var x = document.cookie;
      var decodedCookie = decodeURIComponent(x);
      var bufferSize = lengthBytesUTF8(decodedCookie) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(decodedCookie,buffer,bufferSize);
      return buffer;
  },

  setCookies: function(cname, cvalue){
    var d = new Date();
    d.setTime(d.getTime() + (24*60*60*1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = Pointer_stringify(cname) + "=" +Pointer_stringify(cvalue) + ";" + expires + ";";
  },
});