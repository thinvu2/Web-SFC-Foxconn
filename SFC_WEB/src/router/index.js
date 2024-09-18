import { createRouter, createWebHashHistory } from 'vue-router'
import Home from '../views/Home.vue'
import Login from '../views/Login.vue'
import SOP from '../views/SOP.vue'
const routes = [{
    path: '/',
    alias: ['/', '/Home', '/Home/'],
    name: 'Home',
    component: Home,
    children: [
        {
            path: '/Home/Applications',
            component: () =>
                import('../views/Applications.vue')
        },
        {
        path: '/Home/QM',
        component: () =>
            import('../views/QM.vue')
    }, {
        path: '/Home/Ambit',
        component: () =>
            import('../views/Ambit.vue')
    },
    {
        path: '/',
        component: () =>
            import('../views/DashBoard.vue')
    },
    {
        path: '/Home/LockStation',
        name: 'LockStation',
        component: () =>
            import('../views/LockStation.vue'),
    },
    {
        path: '/Home/LockHistory',
        name: 'LockHistory',
        component: () =>
            import('../views/LockHistory.vue'),
    },
    {
        path: '/Home/Query6',
        name: 'Query6',
        component: () =>
            import('../views/Query6.vue'),
    },
    {
        path: '/Home/QueryINV2',
        name: 'QueryINV2',
        component: () =>
            import('../views/QueryINV2.vue'),
    },    
    {
        path: '/Home/GERENAL_QUERY',
        name: 'GERENAL_QUERY',
        component: () =>
            import('../views/GERENAL_QUERY.vue'),
    },       
    {
        path: '/Home/SHIPTOFILE',
        name: 'QUERY_GERENAL',
        component: () =>
            import('../views/ShipToFile.vue'),
    },      
    {
        path: '/Home/SHIPDATA',
        name: 'SHIPDATA',
        component: () =>
            import('../views/ShipData.vue'),
    },
    {
        path: '/Home/SAGEMCOM',
        name: 'SAGEMCOM',
        component: () =>
            import('../views/Sagemcom.vue'),
    },
    {
        path: '/Home/SAGEMCOMFOX',
        name: 'SAGEMCOMFOX',
        component: () =>
            import('../views/SagemcomFox.vue'),
    },
    {
        path: '/Home/TCQS',
        name: 'TCQS',
        component: () =>
            import('../views/TCQS.vue'),
    },
    {
        path: '/Home/BonePile',
        name: 'BONEPILE',
        component: () =>
            import('../views/BonePile.vue'),
    },
    {
        path: '/Home/ModelDestT2',
        name: 'ModelDestT2',
        component: () =>
            import('../views/ModelDestT2.vue'),
    },
    {
        path: '/Home/RepairSearch',
        name: 'RepairSearch',
        component: () =>
            import('../views/RepairSearch.vue'),
    },
    {
        path: '/Home/ConfigApp',
        name: 'ConfigApp',
        component: () =>
            import('../views/NewConfigApp.vue'),
    },
    {
        path: '/Home/TrapTest',
        name: 'TrapTest',
        component: () =>
            import('../views/TrapTest.vue'),
    },   
    
    {
        path: '/Home/QueryModel',
        name: 'QueryModel',
        component: () =>
            import('../views/QueryModel.vue'),
    },

    {
        path: '/Home/PQE_NIC',
        name: 'PQE_NIC',
        component: () =>
            import('../views/Pqe_nic.vue'),
    },
    
    {
        path: '/Home/PQE_NIC/Make_WeightConfig',
        name: 'Make_WeightConfig',
        component: () =>
            import('../components/ConfigComponents/Make_WeightConfig.vue'),
    },    
    {
        path: '/Home/ConfigApp/MODEL_ATTRCONFIG',
        name: 'MODEL_ATTRCONFIG',
        component: () =>
            import('../components/ConfigComponents/MODEL_ATTRCONFIG.vue'),
    },    
    {
        path: '/Home/PQE_NIC/CONFIRM_CUST_MODEL',
        name: 'CONFIRM_CUST_MODEL',
        component: () =>
            import('../components/ConfigComponents/ConfirmCust_Model.vue'),
    },    
    {
        path: '/Home/ConfigApp/CHECK_ROUTE',
        name: 'CHECK_ROUTE',
        component: () =>
            import('../components/ConfigComponents/CheckRoute.vue'),
    },
    {
        path: '/Home/ConfigApp/Allparts_StationConfig',
        name: 'Allparts_StationConfig',
        component: () =>
            import('../components/ConfigComponents/Allparts_StationConfig.vue'),
    },
    {
        path: '/Home/PQE_TELIT',
        name: 'PQE_TELIT',
        component: () =>
            import('../views/Pqe_Telit.vue'),
    },
    {
        path: '/Home/PQE_TELIT/ROAST_TIME',
        name: 'ROAST_TIME',
        component: () =>
            import('../components/ConfigComponents/Roast_time.vue'),
    },    
    {
        path: '/Home/QueryRepair',
        name: 'QueryRepair',
        component: () =>
            import('../views/QueryRepair.vue'),
    },
    {
        path: '/Home/BakeNO',
        name: 'BakeNO',
        component: () =>
            import('../views/BakeNO.vue'),
    },
    {
        path: '/Home/ConfigApp/Config1',
        component: () =>
            import('../components/ConfigComponents/Config1.vue'),
    },    
    {
        path: '/Home/ConfigApp/Config2',
        component: () =>
            import('../components/ConfigComponents/Config2.vue'),
    },
    {
        path: '/Home/ConfigApp/Config4',
        component: () =>
            import('../components/ConfigComponents/Config4.vue'),
    },
    {
        path: '/Home/ConfigApp/Config6',
        component: () =>
            import('../components/ConfigComponents/Config6.vue'),
    },
    {
        path: '/Home/ConfigApp/Config7',
        component: () =>
            import('../components/ConfigComponents/Config7.vue'),
    },
    {
        path: '/Home/ConfigApp/Config8',
        component: () =>
            import('../components/ConfigComponents/Config8.vue'),
    },
    {
        path: '/Home/ConfigApp/Config9',
        component: () =>
            import('../components/ConfigComponents/Config9.vue'),
    },
    {
        path: '/Home/ConfigApp/Config10',
        component: () =>
            import('../components/ConfigComponents/Config10.vue'),
    },
    {
        path: '/Home/ConfigApp/Config11',
        component: () =>
            import('../components/ConfigComponents/Config11.vue'),
    },
    {
        path: '/Home/ConfigApp/Config12',
        component: () =>
            import('../components/ConfigComponents/Config12.vue'),
    },
    {
        path: '/Home/ConfigApp/Config14',
        component: () =>
            import('../components/ConfigComponents/Config14.vue'),
    },
    {
        path: '/Home/ConfigApp/Config15',
        component: () =>
            import('../components/ConfigComponents/Config15.vue'),
    },
    {
        path: '/Home/ConfigApp/Config19',
        component: () =>
            import('../components/ConfigComponents/Config19.vue'),
    },
    {
        path: '/Home/ConfigApp/Config21',
        component: () =>
            import('../components/ConfigComponents/Config21.vue'),
    },
    {
        path: '/Home/ConfigApp/Config23',
        component: () =>
            import('../components/ConfigComponents/Config23.vue'),
    },
    {
        path: '/Home/ConfigApp/Config40',
        component: () =>
            import('../components/ConfigComponents/Config40.vue'),
    },
    {
        path: '/Home/ConfigApp/Config42',
        component: () =>
            import('../components/ConfigComponents/Config42.vue'),
    },
    {
        path: '/Home/ConfigApp/Config43',
        component: () =>
            import('../components/ConfigComponents/Config43.vue'),
    },
    {
        path: '/Home/ConfigApp/Config44',
        component: () =>
            import('../components/ConfigComponents/Config44.vue'),
    },
    {
        path: '/Home/ConfigApp/Config45',
        component: () =>
            import('../components/ConfigComponents/Config45.vue'),
    },
    {
        path: '/Home/ConfigApp/Config46',
        component: () =>
            import('../components/ConfigComponents/Config46.vue'),
    },
    {
        path: '/Home/ConfigApp/Config47',
        component: () =>
            import('../components/ConfigComponents/Config47.vue'),
    },
    {
        path: '/Home/ConfigApp/Config50',
        component: () =>
            import('../components/ConfigComponents/Config50.vue'),
    },  
    {
        path: '/Home/ConfigApp/Config52',
        component: () =>
            import('../components/ConfigComponents/Config52.vue'),
    },  
    {
        path: '/Home/ConfigApp/Config53',
        component: () =>
            import('../components/ConfigComponents/Config53.vue'),
    },   
    {
        path: '/Home/ConfigApp/Config54',
        component: () =>
            import('../components/ConfigComponents/Config54.vue'),
    },
    {
        path: '/Home/ConfigApp/Config60',
        component: () =>
            import('../components/ConfigComponents/Config60.vue'),
    },
    {
        path: '/Home/ConfigApp/Config64',
        component: () =>
            import('../components/ConfigComponents/Config64.vue'),
    },    
    {
        path: '/Home/ConfigApp/Config65',
        component: () =>
            import('../components/ConfigComponents/Config65.vue'),
    },
    {
        path: '/Home/ConfigApp/Config67',
        component: () =>
            import('../components/ConfigComponents/Config67.vue'),
    },
    {
        path: '/Home/ConfigApp/Config69',
        component: () =>
            import('../components/ConfigComponents/Config69_.vue'),
    },
    {
        path: '/Home/ConfigApp/Config76',
        component: () =>
            import('../components/ConfigComponents/Config76.vue'),
    },
    {
        path: '/Home/ConfigApp/Config82',
        component: () =>
            import('../components/ConfigComponents/Config82.vue'),
    },
    {
       path: '/Home/ConfigApp/Config74',
       component: () =>
           import('../components/ConfigComponents/Config74.vue'),
   },
     //HAU ADD CONFIG 76
     {
        path: '/Home/ConfigApp/Config76_',
        component: () =>
            import('../components/ConfigComponents/Config76_.vue'),
    },
    {
        path: '/Home/ConfigApp/Config88',
        component: () =>
            import('../components/ConfigComponents/Config88.vue'),
    },
    {
        path: '/Home/ConfigApp/Config88_setup',
        component: () =>
            import('../components/ConfigComponents/Config88_setup.vue'),
    },
    {
        path: '/Home/ConfigApp/Config88_combine',
        component: () =>
            import('../components/ConfigComponents/Config88_combine.vue'),
    },
    {
        path: '/Home/ConfigApp/Label3s',
        component: () =>
            import('../components/ConfigComponents/Label3s.vue'),
    },    
    {
        path: '/Home/ConfigApp/Label_lssc',
        component: () =>
            import('../components/ConfigComponents/Label_lssc.vue'),
    },
    {
        path: '/Home/ConfigApp/WeightSetup',
        component: () =>
            import('../components/ConfigComponents/WeightSetup.vue'),
    },
    {
        path: '/Home/ConfigApp/ConfigJIG',
        component: () =>
            import('../components/ConfigComponents/ConfigJIG.vue'),
    },
    {
        path: '/Home/ConfigApp/ConfigPO',
        component: () =>
            import('../components/ConfigComponents/ConfigPO.vue'),
    },
    {
        path: '/Home/ConfigApp/Config74/CheckLog',
        component: () =>
            import('../components/ConfigComponents/CheckLog.vue'),
    },    
    {
        path: '/Home/ConfigApp/HSN_Config',
        component: () =>
            import('../components/ConfigComponents/HSN_Config.vue'),
    },    
    {
        path: '/Home/ConfigApp/Route_Bom',
        component: () =>
            import('../components/ConfigComponents/Route_Bom.vue'),
    },
    {
        path: '/Home/ConfigApp/Invoice_Ship',
        component: () =>
            import('../components/ConfigComponents/Invoice_Ship.vue'),
    },
    {
        path: '/Home/PQE_NIC/ConfirmLabel_Qty',
        component: () =>
            import('../components/ConfigComponents/ConfirmLabel_Qty.vue'),
    }, 
    {
        path: '/Home/PQE_TELIT/LABEL_MSL',
        component: () =>
            import('../components/ConfigComponents/ConfirmLabel_MSL.vue'),
    }, 
    {
        path: '/Home/ConfigApp/Privilege',
        component: () =>
            import('../components/ConfigComponents/Privilege.vue'),
    },
    {
        path: '/Home/UploadData',
        name: 'UploadData',
        component: () =>
            import('../views/UploadData.vue'),
    },
    {
        path: '/Home/LINKDN',
        name: 'LINKDN',
        component: () =>
            import('../views/LINKDN.vue'),
    },    
    {
        path: '/Home/ImportExcel',
        name: 'ImportExcel',
        component: () =>
            import('../views/ImportExcel.vue'),
    },
    ]
},
{
    path: '/Login',
    name: 'Login',
    component: Login,
},
{
    path: '/SOP',
    name: 'SOP',
    component: SOP,
},
// {
//     path: '/LockStation',
//     name: 'LockStation',
//     component: () =>
//     import ('../views/LockStation.vue'),
// },
{
    path: '/about',
    name: 'About',
    component: () =>
        import('../views/About.vue')
},

]

const router = createRouter({
    history: createWebHashHistory(),
    routes
})

export default router