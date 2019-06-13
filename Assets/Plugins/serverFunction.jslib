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
      var bufferSize = lengthBytesUTF8(x) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(x,buffer,bufferSize);
      return buffer;
  },
});