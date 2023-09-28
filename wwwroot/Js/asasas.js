function onFileSelected() {
    let profile = "";
    const inputNode: any = document.getElementById('ProfileName');

    if (typeof FileReader !== 'undefined') {
        const reader = new FileReader();

        reader.onload = (e: any) => {
            profile = e.target.result;

            document
                .getElementById('ProfileImg')
                ?.setAttribute('src', this.profile);
        };
        reader.readAsDataURL(inputNode.files[0]);
    }
}