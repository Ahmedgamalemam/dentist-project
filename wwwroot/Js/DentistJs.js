﻿function sidebar() {
    document.getElementById('side')?.classList.toggle('show-side');
} 
function hidenave() {
    document.getElementById('navbar')?.classList.add('hide_nav');
    document.getElementById('footer')?.classList.add('hide_footer');
}
function shownave() {
    document.getElementById('navbar')?.classList.remove('hide_nav');
    document.getElementById('footer')?.classList.remove('hide_footer');
}
function settolocalstorage() {
    document.getElementById('navbar')?.classList.remove('hide_nav');
}
function show_div() {
    let divs = document.querySelectorAll('.div');
    divs.forEach((item) => {
        item.classList.add("show_div");
    });
}
