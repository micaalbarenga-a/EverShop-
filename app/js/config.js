'use strict';


app.config(function ($urlRouterProvider, $stateProvider) {

    $urlRouterProvider.otherwise('/login');
    $stateProvider
        .state('login', {
            url: "/login",
            controller: 'MainCtrl',
            templateUrl: "/app/views/login.html"
        })
        .state('create', {
            url: "/create",
            controller: 'CreateCtrl',
            templateUrl: "/app/views/create-account.html"
        })

        .state('index', {
            url: "/index",
            controller: 'IxCtrl',
        })

        //.state('index', {
        //    url: "/index",
        //    controller: '',
        //    templateUrl: "/index.html"
        //})

        .state('neworder', {
            url: "/neworder",
            controller: "NewOrderCtrl",
            templateUrl: "/app/modules/new.order/view/main.html"
        })

        .state('resume', {
            url: "/resume",
            controller: "ResumeCtrl",
            templateUrl: "/app/modules/resume.order/view/main.html",
            params: { order: null }
        })


        .state('payed', {
            url: "/payed/:id",
            controller: "PayedCtrl",
            templateUrl: "/app/modules/payed/view/main.html",
        })

        .state('myorders', {
            url: "/myorders",
            controller: "MyOrdersCtrl",
            templateUrl: "/app/modules/my.orders/view/main.html",
        })

        .state('orders', {
            url: "/orders",
            controller: "OrdersCtrl",
            templateUrl: "/app/modules/orders/view/main.html",
        })

})