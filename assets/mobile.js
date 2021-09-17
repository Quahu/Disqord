if (navigator.userAgentData && navigator.userAgentData.mobile || navigator.userAgent.match(/Mobi/i)) {
    if (navigator.userAgent.match(/iPhone/i)) {
        document.documentElement.style.height = "-webkit-fill-available";
        document.body.style.minHeight = "-webkit-fill-available";
    }

    const leftColumn = document.querySelector("#left-col");
    const main = document.querySelector("main");
    const firstBreadcrumb = document.querySelectorAll(".breadcrumb-item").item(0);
    if (firstBreadcrumb && firstBreadcrumb.classList.contains("active")) {
        main.append(...leftColumn.childNodes);
        document.querySelector("#tutorial-tree").classList.remove("sticky-top");
    }

    leftColumn.remove();
    main.classList.remove("w-50", "d-flex");
    document.querySelector("#right-col").remove();
}
