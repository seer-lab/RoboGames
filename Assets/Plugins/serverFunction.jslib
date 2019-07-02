mergeInto(LibraryManager.library, {

  GetData: function (url) {
    try{
      var xmlHttp = new XMLHttpRequest();
      xmlHttp.open( "GET", Pointer_stringify(url), false ); // false for synchronous request
      xmlHttp.send( null );
      var text = xmlHttp.responseText;
      var bufferSize = lengthBytesUTF8(text) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(text, buffer, bufferSize);
    }catch(e){
      var bufferSize = lengthBytesUTF8("") + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8("", buffer, bufferSize);
      return buffer;
    }
    return buffer;
  },
});