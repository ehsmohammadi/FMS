function recalculateReportViewerLayout(id) {
    var viewer = $find(id);
    if(viewer != null && !viewer.isLoading && !viewer.get_isLoading())
        //viewer.recalculateLayout();
        viewer.refreshReport();
}