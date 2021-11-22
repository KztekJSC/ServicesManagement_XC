var language = {
    init(path) {
        
    },

    async getText(path) {
        var url = '/Home/GetText';
        var model = { path: path };
        return await JSHelper.AJAX_LoadDataAsync(url, model)
    }
}