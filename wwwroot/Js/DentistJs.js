function sidebar() {
    document.getElementById('side')?.classList.toggle('show-side');
}
function hidenave() {
    document.getElementById('navbar')?.classList.add('hide_nav');
}
function shownave() {
    document.getElementById('navbar')?.classList.remove('hide_nav');
}
function settolocalstorage() {
    document.getElementById('navbar')?.classList.remove('hide_nav');
}