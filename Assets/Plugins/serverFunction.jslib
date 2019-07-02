mergeInto(LibraryManager.library, {

  GetData: function (url) {
    try{
<<<<<<< HEAD
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open( "GET", Pointer_stringify(url), false ); // false for synchronous request
    xmlHttp.send( null );
    var text = xmlHttp.responseText;
    var bufferSize = lengthBytesUTF8(text) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(text, buffer, bufferSize);
    }catch(e){
      return "";
    }
    return buffer;
  },
  
  getCookies: function () {
      var x = document.cookie;
      var decodedCookie = decodeURIComponent(x);
      var bufferSize = lengthBytesUTF8(decodedCookie) + 1;
=======
      var xmlHttp = new XMLHttpRequest();
      xmlHttp.open( "GET", Pointer_stringify(url), false ); // false for synchronous request
      xmlHttp.send( null );
      var text = xmlHttp.responseText;
      var bufferSize = lengthBytesUTF8(text) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(text, buffer, bufferSize);
    }catch(e){
      var bufferSize = lengthBytesUTF8("") + 1;
>>>>>>> af4580f62ea0b6c451809902c62c1841c5b3551a
      var buffer = _malloc(bufferSize);
      stringToUTF8("", buffer, bufferSize);
      return buffer;
    }
    return buffer;
  },
});