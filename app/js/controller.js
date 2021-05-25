'use strict'
app.factory('utils', function ($http, CONFIG) {
    let urlBase = CONFIG.ct_URL;
    return {
        login: (u) => $http.post(urlBase + '/login/login', u),
        logout: () => $http.post(urlBase + '/login/logout'),
        addUser: (u) => $http.post(urlBase + '/login/addUser', u),
    }

})
    .controller("MainCtrl", function ($scope, utils, toaster, $state, $rootScope) {
        var m = $scope.m = {};

        //User Agent
        $rootScope.userAgent = navigator.userAgent;


        //Log in
        m.login = () => {
            m.isLoading = true;
            utils.login(m.cus).then(
                ({ data }) => {
                    //m.adm => puede ver listado de todas las órdenes de compra
                    $rootScope.adm = data;
                    m.isLoading = false;
                    $rootScope.log = true;
                    $state.go('index');
                },

                ({ data }) => {
                    m.isLoading = false;
                    toaster.warning(data.Message);
                }


            );
        }

        //Log out
        m.logout = () => {
            m.isLoading = true;
            utils.logout().then(
                ({ data }) => {
                    m.isLoading = false;
                    $state.go('login');
                    $rootScope.log = false;
                },

                ({ data }) => {
                    m.isLoading = false;
                    $rootScope.log = false;
                    $state.go("login");
                }
            );
        }

    })

    .controller("CreateCtrl", function($scope, $state, utils) {
        let c = $scope.c = {};
        //Alta de usuarios
        c.create = (u) => {
            c.isLoading = true;
            utils.addUser(u).then(
                ({ data }) => {
                    c.isLoading = false;
                    $state.go('login');
                },

                ({ data }) => {
                    m.isLoading = false;
                    $state.go("login");
                }
            );
        }

    })


    .controller("IxCtrl", function ($state, $rootScope) {
        if (!$rootScope.log)
            $state.go("login");
    })

    ;
