/*!
* Start Bootstrap - Simple Sidebar v6.0.3 (https://startbootstrap.com/template/simple-sidebar)
* Copyright 2013-2021 Start Bootstrap
* Licensed under MIT (https://github.com/StartBootstrap/startbootstrap-simple-sidebar/blob/master/LICENSE)
*/
// 
// Scripts
// 

window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

    document.querySelector('#nroSerieGlobal').addEventListener('keyup', () => {
        SetNroSerieAllLinks();
    });
});


function SetNroSerieAllLinks() {
    var allLinks = document.querySelectorAll('a.nav-link');
    var noSerieGlobal = document.querySelector('#nroSerieGlobal').value;
    allLinks.forEach(link => {
        var href = link.href.toString();
        if (!href.includes('#') && !href.includes('BaseTemplate')) {
            if ($.trim(noSerieGlobal) != '' && $.trim(noSerieGlobal).length > 0) {
                var baseHref = href.split('?noSerie=');
                var newHref = baseHref[0].concat('?noSerie=', noSerieGlobal.toUpperCase());
                link.href = newHref;
            } else {
                if (href.includes('?noSerie=')) {
                    var splitedfHref = href.split('?');
                    var newHref = splitedfHref[0];
                    link.href = newHref;
                }
            }
        }
    });
}