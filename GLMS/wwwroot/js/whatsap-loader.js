(function () {
    /*
     * @info get data attribute
     * 
    */
    const { styleDir } = document.currentScript.dataset;
    if (styleDir) {
        const styleTag = document.createElement("link");
        styleTag.href = styleDir;
        styleTag.rel = "stylesheet";
        // if file not found!
        styleTag.onerror = function (e) {
            const fixedError = document.createElement("div");
            const error = document.createElement("p");
            error.style.margin = "auto";
            error.textContent = "whatsapp-loader-js error! check your script data style attribute!";
            fixedError.style.width = "100%";
            fixedError.style.height = "100vh";
            fixedError.style.position = "fixed";
            fixedError.style.left = "0";
            fixedError.style.top = "0";
            fixedError.style.backgroundColor = "white";
            fixedError.style.color = "red";
            fixedError.style.zIndex = "99999999";
            fixedError.style.display = "flex";
            fixedError.style.alignItems = "center";
            fixedError.append(error);
            document.body.prepend(fixedError);
        }
        document.head.append(styleTag);
    }
    window.showLoading = () => {
        document.body.classList.add("_ovh");
        document.body.prepend(getLoaderTemplate());
    }
    window.hideLoading = () => {
        document.body.classList.remove("_ovh");
        document.getElementById("loader").remove();
    }
    function getLoaderTemplate() {
        const loader_template = document.createElement("div");
        loader_template.id = "loader";
        loader_template.innerHTML = `<div>
            <ul>
                <li></li>
                <li></li>
                <li></li>
                <li></li>
                <li></li>
            </ul>
        </div>`;
        return loader_template;
    }    
})()
