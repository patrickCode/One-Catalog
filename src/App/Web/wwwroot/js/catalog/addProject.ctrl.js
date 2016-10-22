﻿(function (module) {

    var addProjectController = function ($scope, $rootScope, $location, adalAuthenticationService, catalogData, technologyData) {

        var addProject = function () {
            $scope.submitting = true;

            $scope.techTags.forEach(function (tag) {
                var existingTech = _.findWhere($scope.project.technologies, { name: tag.text });
                if (existingTech === undefined || existingTech === null)
                    $scope.project.technologies.push({ "id": 0, "name": tag.text.replace(/-/g, ' '), "relatedTechologies": null });
            });

            $scope.contactTags.forEach(function (contact) {
                var existingContact = _.findWhere($scope.project.contacts, { alias: contact.text });
                if (existingContact === undefined || existingContact === null)
                    $scope.project.contacts.push({ "alias": contact.text, "name": "" });
            });

            $scope.errorOcurred = false;
            catalogData.addNewProject($scope.project)
                .then(function (data) {
                    $scope.submitting = false;
                    var projectId = data;
                    $location.path("home/" + projectId + "/details");
                }, function (error) {
                    $scope.submitting = false;
                    alert("Some unexpected error ocurred");
                });
        }

        $scope.getTechnologySuggestions = function (query) {
            return technologyData.getTechnologySuggestions(query);
        }

        var init = function() {
            $scope.submit = addProject;

            $scope.techTags = [];
            $scope.contactTags = [{ "text": adalAuthenticationService.userInfo.userName }];

            $scope.submitting = false;
            $scope.project = {
                "id": 0,
                "name": "",
                "description": "",
                "abstract": "",
                "additionalDetails": "",
                "technologies": [],
                "createdBy": { "alias": adalAuthenticationService.userInfo.userName, "name": "" },
                "contacts": [{ "alias": adalAuthenticationService.userInfo.userName, "name": "" }],
                "codeLink": { "linkType": "Code", "href": null, "desciption": null },
                "previewLink": { "linkType": "Preview", "href": null, "desciption": null },
                "additionalLinks": [],
                "createdOn": null,
                "lastModifiedOn": null,
                "lastModifiedBy": { "alias": adalAuthenticationService.userInfo.userName, "name": "" },
                "isDeleted": false
            }
        }
        init();
    }

    module.controller("addProjectController", ["$scope", "$rootScope", "$location", "adalAuthenticationService", "catalogData", "technologyData", addProjectController]);

}(angular.module("catalog")))