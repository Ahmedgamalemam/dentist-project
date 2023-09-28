function active_delete(event) {
    let target_element = event.target;
    target_element.classList.toggle('active_delete');
    target_element?.parentElement?.classList.toggle('active_delete');

    let after = document.querySelectorAll('.active_delete');
    after.forEach((element) => {
        if (
            element != target_element &&
            element != target_element?.parentElement
        ) {
            element.classList.remove('active_delete');
            element?.parentElement?.classList.remove('active_delete');
        }
    });
}
function getbyid(id) {
    return document.getElementById(id)
}
function getbyidvalue(id) {
    return document.getElementById(id).innerText
}
function queryselectall() {
    let active_delete = document.querySelectorAll('.active_delete');
    active_delete.forEach((element: any) => {
        element.classList.remove('active_delete');
    });
}
function openside() {
    let openside = document.getElementById('openside');
    openside?.classList.remove('openside');
    let closeside = document.getElementById('closeside');
    closeside?.classList.add('closeside');
    let side = document.getElementById('side');
    side?.classList.add('showside');
}
function closeside() {
    let openside = document.getElementById('openside');
    openside?.classList.add('openside');
    let closeside = document.getElementById('closeside');
    closeside?.classList.remove('closeside');
    let side = document.getElementById('side');
    side?.classList.remove('showside');
}
function hideDelete_Update(e) {
    let divToHide = document.getElementById('update_and_delete');
    if (divToHide == e.target) {
        this.update_message = null;
        this.delete_message = null;
    }
}
function filteredbutton(id){
    let element = this.getbyidvalue(id)
    element?.classList.add('back_purple_dark');
}
function scroll(id) {
    this.getbyid(id).scrollIntoView()
}