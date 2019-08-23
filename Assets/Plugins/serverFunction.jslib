mergeInto(LibraryManager.library, {

  GetData: function (url) {
    try{
      var xmlHttp = new XMLHttpRequest();
      xmlHttp.open( "GET", Pointer_stringify(url), false ); // false for synchronous request
      xmlHttp.send( null );
      var text = xmlHttp.responseText;
      if(xmlHttp.status == 404 ){
        text = "File not found!";
      }
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

  //Yoinked from https://forum.unity.com/threads/videoplayer-webgl-from-streamingassets-to-indexeddb.527527/
  SaveVideoInIndexedDB : function (path, idVideo) {
    var jPath = Pointer_stringify(path);
    var jIdVideo = Pointer_stringify(idVideo);
    window.indexedDB = window.indexedDB || window.webkitIndexedDB || window.mozIndexedDB || window.OIndexedDB || window.msIndexedDB,
    IDBTransaction = window.IDBTransaction || window.webkitIDBTransaction || window.OIDBTransaction || window.msIDBTransaction,
      dbVersion = 1.0;
    if (!window.indexedDB)
      {
        alert("Your browser doesn't support IndexedDB");
                      return -1;
    }
    var request = indexedDB.open("YourDB", dbVersion),
    db,
    createObjectStore = function (dataBase)
    {
          dataBase.createObjectStore("Videos");
      },
      getVideoFile = function ()
      {
      var xhr = new XMLHttpRequest(),
        blob;
        // Get the Video file from the server.
        xhr.open("GET", jPath, true);  
        xhr.responseType = "blob";
        xhr.addEventListener("load", function ()
        {
            if (xhr.status === 200)
            {
                blob = xhr.response;
                console.log("SUCCESS: Video file downloaded " + jIdVideo)
                putVideoInDb(blob);
            }
            else
            {
                console.log("ERROR: Unable to download video.")
            }
          }, false);
          xhr.send();
        },
        putVideoInDb = function (blob) {
          var transaction = db.transaction(["Videos"], "readwrite");
          var put = transaction.objectStore("Videos").put(blob, jIdVideo);
        };
    request.onerror = function (event) {
            console.log("IndexedDB error: " + event.target.errorCode);
      };
  
      request.onsuccess = function (event) {
        console.log("Success creating/accessing IndexedDB database");
      db = request.result;
          db.onerror = function (event) {
              console.log("Error creating/accessing IndexedDB database");
          };      
          window.onload = getVideoFile();
      }
    request.onupgradeneeded = function (event) {
          createObjectStore(event.target.result);
      };
  },

  //Yoinked from https://forum.unity.com/threads/videoplayer-webgl-from-streamingassets-to-indexeddb.527527/
  GetUrlFromIndexedDB: function (str) {
    return new Promise(function(resolve) {
      var jStr = Pointer_stringify(str);
      var finUrl = "";
      console.log("jSTR : " + jStr);
      window.indexedDB = window.indexedDB || window.webkitIndexedDB || window.mozIndexedDB || window.OIndexedDB || window.msIndexedDB,
      IDBTransaction = window.IDBTransaction || window.webkitIDBTransaction || window.OIDBTransaction || window.msIDBTransaction,
      dbVersion = 1.0;
      if (!window.indexedDB){
        alert("Your browser doesn't support IndexedDB");
      }
      var indexedDB = window.indexedDB;

      var request = indexedDB.open("YourDB", dbVersion);

      request.onerror = function (event) {
          console.log("IndexedDB error: " + event.target.errorCode);

      };

      request.onsuccess = function (event) {
        db = request.result;      
        // Open a transaction to the database
        var transaction = db.transaction(["Videos"], "readwrite");
        // Retrieve the video file
        transaction.objectStore("Videos").get(jStr).onsuccess = function (event) {              
          var videoFile = event.target.result;
          var URL = window.URL || window.webkitURL;
          try{
            videoURL = (window.URL ? URL : webkitURL).createObjectURL(videoFile);
          }catch(e){
            console.log("Your Browser doesnt support createObjectURL!");
            videoURL = "";
          }
          SendMessage('GameObject', 'SetMovieIndexedDBURL', videoURL + " " + jStr);
        }
      };
    });
  },
});