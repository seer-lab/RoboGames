function UnityProgress(gameInstance, progress) {
    if (!gameInstance.Module)
        return;
    if (!gameInstance.logo) {
        gameInstance.logo = document.createElement("div");
        gameInstance.logo.className = "logo " + gameInstance.Module.splashScreenStyle;
        gameInstance.container.appendChild(gameInstance.logo);
    }

    if (progress == "complete") {
        gameInstance.logo.style.display = "none";
        $('#overlay').delay(1000).animate({
            "margin-left": '-=1700'
        }, 'slow').fadeOut();
        $('#ProgressLine').delay(200).animate({
            "margin-top": '-=2000'
        }, 'slow').fadeOut();
        $('#loadingInfo').delay(200).animate({
            "margin-top": '-=2000'
        }, 'slow').fadeOut();
        $('#progressC').delay(200).animate({
            "margin-top": '-=2000'
        }, 'slow').fadeOut();
        $('#loadingBox').delay(200).animate({
            "margin-top": '-=2000'
        }, 'slow');
        document.getElementById("ProgressLineB").style.display = "none";
        return;
    } else if (progress == 1) {
        document.getElementById("ProgressLine").style.width = (200 * (progress)) + "px";
        document.getElementById("loadingInfo").innerHTML = "loading";
        document.getElementById("progressC").innerHTML = Math.floor((100 * progress)) + "%";
    } else {
        document.getElementById("loadingInfo").innerHTML = "downloading";
        document.getElementById("progressC").innerHTML = Math.floor((100 * progress)) + "%";
        document.getElementById("ProgressLine").style.width = (200 * (progress)) + "px";
    }

}