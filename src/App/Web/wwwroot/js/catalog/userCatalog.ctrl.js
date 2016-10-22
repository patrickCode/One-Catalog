(function (module) {

    var userCatalogController = function ($scope, $rootScope, $location, catalogData, adalAuthenticationService) {

        var getUserProjects = function (reset) {
            $scope.loading = true;
            $scope.errorOcurred = false;
            if (reset !== undefined && reset !== null && reset === true)
                $scope.pagination.currentPage = 1;
            catalogData.getProjectsByUser($scope.userId, $scope.searchText, $scope.pagination.currentPage, $scope.pagination.pageSize)
                .then(function (data) {
                    $scope.projects = data.results;
                    $scope.count = data.totalCount;
                    $scope.loading = false;
                    repaginate();
                }, function (error) {
                    alert("Some unexpected error ocurred");
                });
            //.error(function (error) {
            //    $scope.errorMessage = "Some error ocurred";
            //    $scope.errorOcurred = true;
            //    $scope.loading = false;
            //});
        }

        var repaginate = function () {
            var totalPages = parseInt(parseInt($scope.count / $scope.pagination.pageSize) + ($scope.count % $scope.pagination.pageSize !== 0 ? 1 : 0));
            $scope.pages = Array.apply(null, { length: totalPages }).map(Number.call, Number);

            if ($scope.pagination.currentPage > totalPages)
                $scope.pagination.currentPage = 1;
        }

        var goToDetails = function (project) {
            $location.path("home/" + project.id + "/details");
        }

        var editProject = function (project) {
            $location.path("home/" + project.id + "/edit");
        }

        var changePage = function (currentPage) {
            if (currentPage > 0 || currentPage <= $scope.pages.length) {
                $scope.pagination.currentPage = currentPage;
                getUserProjects();
            }
        }

        var init = function () {
            $rootScope.breadcrumbs = $rootScope.breadcrumbs || [{}];
            $rootScope.breadcrumbs.pop();
            $rootScope.breadcrumbs.push({
                title: "User Catalog",
                href: "/catalog"
            });
            $scope.searchText = "";
            $scope.userId = adalAuthenticationService.userInfo.userName;

            $scope.pagination = {
                currentPage: 1,
                pageSize: 10
            };
            getUserProjects(true);
            $scope.search = getUserProjects;
            $scope.changePage = changePage;
            $scope.goToDetails = goToDetails;
            $scope.editProject = editProject;
        }
        init();
    }

    module.controller("userCatalogController", ["$scope", "$rootScope", "$location", "catalogData", "adalAuthenticationService", userCatalogController])

}(angular.module("catalog")));