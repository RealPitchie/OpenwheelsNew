 
     function scrollIntoView (elementId) { 
        var elem = document.getElementById(elementId);
        if (elem) {
            elem.scrollIntoView({ behavior: "smooth" });
            window.location.hash = elementId;
        }
    } 