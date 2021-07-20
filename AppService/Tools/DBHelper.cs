using AppService.Models;
using Dapper;
using DapperExtensions;
using MSTL.DbClient;
using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using AppService.Tools;
using DapperExtensions.Mapper;
using System.Threading;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace AppService.Tools
{
    public class DBHelper
    {
        private string objidPalletInWare;
        private string objidBound;
        private string orderNo;
        private IDatabase db;
        private IDatabase dbl;
        public string constr = McConfig.Instance.getConfig("SqlConnect");
        private static DBHelper _instance = null;
        /// <summary>
        /// 单例模式
        /// </summary>
        public static DBHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(DbHelper))
                    {
                        if (_instance == null)
                        {
                            _instance = new DBHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        public DBHelper()
        {
            //用来连接sqlserver数据库
            string errMsg = string.Empty;
            try
            {
                dbl = DbHelper.GetDb(constr, DbHelper.DataBaseType.SqlServer, ref errMsg);
            }
            catch (Exception ex)
            { }
        }
        /// <summary>
        /// 不同的立库连接不同的数据库
        /// </summary>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        private IDatabase GetDb(string storeNo)
        {
            try
            {
                //if (storeNo == "原材料库")
                //{
                constr = McConfig.Instance.getConfig("SqlASRS");
                //}
                //else if (storeNo == "组装材料库")
                //{
                //    constr = McConfig.Instance.getConfig("SqlPK");
                //}
                //else if (storeNo == "中间库")
                //{
                //    constr = McConfig.Instance.getConfig("SqlDBS");
                //}
                //else
                //{
                //    constr = McConfig.Instance.getConfig("SqlDBS");
                //}
                string errMsg = string.Empty;
                db = DbHelper.GetDb(constr, DbHelper.DataBaseType.SqlServer, ref errMsg);               
                return db;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string UpdatePassWord(string adminusername, string adminpassword, string addusername, string addpassword)
        {
            try
            {
                string resultstr;
                //判断管理员用户名是否正确
                if (adminusername != "admin")
                {
                    return "管理员用户名不正确";
                }
                //判断管理员用户密码是否正确
                var dbLoc = GetDb("中间库");
                var sb = new StringBuilder();
                sb.Append("SELECT * FROM USERS_FOR_ANDROID WHERE USERNAME='admin'");
                var dt = dbLoc.Connection.QueryTable(sb.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultstr = (dt.Rows[0]["PASSWORD"] ?? "").ToString();
                    if (resultstr != adminpassword)
                    {
                        return "管理员密码不正确";
                    }
                    //判断数据库是否已经存在要更改的用户
                    string strSq = "SELECT * FROM USERS_FOR_ANDROID WHERE USERNAME =@USERNAME";
                    DynamicParameters param = new DynamicParameters();
                    param.Add("USERNAME", addusername);
                    var data = dbLoc.Connection.QueryTable(strSq.ToString(), param);
                    if (data != null && data.Rows.Count > 0)
                    {
                        //开始插入用户名密码
                        string strSql = "UPDATE USERS_FOR_ANDROID SET PASSWORD =@PASSWORD , IS_ENABLE = '1'  WHERE USERNAME =@USERNAME";
                        DynamicParameters parame = new DynamicParameters();
                        parame.Add("USERNAME", addusername);
                        parame.Add("PASSWORD", addpassword);
                        dbLoc.Connection.Execute(strSql.ToString(), parame);

                        //获取用户名密码，检查是否更新
                        string strSql1 = "SELECT * FROM USERS_FOR_ANDROID WHERE USERNAME =@USERNAME AND PASSWORD =@PASSWORD";
                        DynamicParameters param1 = new DynamicParameters();
                        param1.Add("USERNAME", addusername);
                        param1.Add("PASSWORD", addpassword);
                        var data1 = dbLoc.Connection.QueryTable(strSql1.ToString(), param1);
                        if (data1 != null && data1.Rows.Count > 0)
                        {
                            return "OK";
                        }
                        return "更新用户名密码失败";
                    }
                    return "数据库没有此用户名";

                }
                else
                {
                    return "数据库没有此管理员用户";
                }

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        /// <summary>
        /// 新增用户名和密码
        /// </summary>
        /// <param name="adminusername"></param>
        /// <param name="adminpassword"></param>
        /// <param name="addusername"></param>
        /// <param name="addpassword"></param>
        /// <returns></returns>
        public string insertPassWord(string adminusername, string adminpassword, string addusername, string addpassword)
        {
            try
            {
                string resultstr;
                //判断管理员用户名是否正确
                if (adminusername != "admin")
                {
                    return "管理员用户名不正确";
                }
                //判断管理员用户密码是否正确
                var dbLoc = GetDb("中间库");
                var sb = new StringBuilder();
                sb.Append("SELECT * FROM USERS_FOR_ANDROID WHERE USERNAME='admin'");
                var dt = dbLoc.Connection.QueryTable(sb.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    resultstr = (dt.Rows[0]["PASSWORD"] ?? "").ToString();
                    if (resultstr != adminpassword)
                    {
                        return "管理员密码不正确";
                    }
                    //判断数据库是否已经存在要添加的用户
                    string strSq = "SELECT * FROM USERS_FOR_ANDROID WHERE USERNAME =@USERNAME";
                    DynamicParameters param = new DynamicParameters();
                    param.Add("USERNAME", addusername);
                    var data = dbLoc.Connection.QueryTable(strSq.ToString(), param);
                    if (data != null && data.Rows.Count > 0)
                    {
                        return "数据库已经存在此用户名";
                    }
                    //开始插入用户名密码
                    string strSql = "INSERT INTO USERS_FOR_ANDROID(USERNAME,PASSWORD,IS_ENABLE,LOGIN_STATUS)VALUES(@USERNAME,@PASSWORD,'1','1')";
                    DynamicParameters parame = new DynamicParameters();
                    parame.Add("USERNAME", addusername);
                    parame.Add("PASSWORD", addpassword);
                    dbLoc.Connection.Execute(strSql.ToString(), parame);
                    //获取用户名密码，检查是否插入
                    string strSql1 = "SELECT * FROM USERS_FOR_ANDROID WHERE USERNAME =@USERNAME AND PASSWORD =@PASSWORD";
                    DynamicParameters param1 = new DynamicParameters();
                    param1.Add("USERNAME", addusername);
                    param1.Add("PASSWORD", addpassword);
                    var data1 = dbLoc.Connection.QueryTable(strSql1.ToString(), param1);
                    if (data1 != null && data1.Rows.Count > 0)
                    {
                        return "OK";
                    }
                    return "插入用户名密码失败";
                }
                else
                {
                    return "数据库没有此管理员用户";
                }

            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        /// <summary>
        /// 用户登陆验证
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWorld"></param>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public bool GetResultFromDB(string userName, string passWorld)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                string strSql = "select * from USERS_FOR_ANDROID where username = @userName and password = @passWorld";
                DynamicParameters param = new DynamicParameters();
                param.Add("userName", userName);
                param.Add("passWorld", passWorld);
                var dt = dbLocal.Connection.QueryTable(strSql, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <returns></returns>
        internal string GetVersionDBMethod()
        {
            string result;
            try
            {
                var dbLocal = GetDb("中间库");
                string strSql = "select t.LAST_VERSION from USERS_FOR_ANDROID t where rownum <=1 and t.LAST_VERSION is not null";
                var dt = dbLocal.Connection.QueryTable(strSql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = (dt.Rows[0]["LAST_VERSION"] ?? "").ToString();
                }
                else
                {
                    result = "";
                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }           

        /// <summary>
        /// 物料绑定
        /// </summary>       
        /// <returns></returns>
        public string BoundingMethod(Bound bound)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                DynamicParameters para = new DynamicParameters();
                para.Add("i_mater_type", bound.MaterType);
                para.Add("i_pallet_type", bound.PalletType);
                para.Add("i_mater_no", bound.MaterNo);
                para.Add("i_pallet_no", bound.PalletNo);
                para.Add("i_line_no", bound.LineNo);
                para.Add("i_rack_no", bound.RackNo);
                para.Add("i_shift_no", bound.ShiftNo);
                para.Add("i_product_qty", bound.ProductQty);
                para.Add("i_creation_by", bound.UserName);
                para.Add("o_product_guid", null, DbType.String, ParameterDirection.Output, size: 50);
                para.Add("o_err_code", null, DbType.String, ParameterDirection.Output, size: 50);
                para.Add("o_err_desc", null, DbType.String, ParameterDirection.Output, size: 50);
                dbLocal.Connection.Execute("proc_1000_bind_product_pda", para, commandType: CommandType.StoredProcedure);
                //var errNo = para.Get<int>("o_err_code");
                var errDesc = para.Get<string>("o_err_desc");

                return errDesc;
            }
            catch (Exception ex)
            {
                this.objidBound = "";
                return ex.Message.ToString();
            }
        }

        /// <summary>
        /// 物料绑定
        /// </summary>       
        /// <returns></returns>
        public string StoreInputMethod(Bound bound)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                DynamicParameters para = new DynamicParameters();              
                para.Add("i_pallet_no", bound.PalletNo);               
                para.Add("i_sloc_no", bound.LocNo);             
                para.Add("o_err_code", null, DbType.String, ParameterDirection.Output, size: 50);
                para.Add("o_err_desc", null, DbType.String, ParameterDirection.Output, size: 50);
                dbLocal.Connection.Execute("proc_1003_store_input", para, commandType: CommandType.StoredProcedure);
                var errNo = para.Get<string>("o_err_code");
                var errDesc = para.Get<string>("o_err_desc");

                return errDesc;
            }
            catch (Exception ex)
            {
                this.objidBound = "";
                return ex.Message.ToString();
            }
        }

        public void ErrRecordGather(string errNo,string errDesc)
        {
            string palletNo = "";
            string materNo = "";
            string materName = "";

            //1.获取出错记录的托盘号、物料号
            var dbLocal = GetDb("原材料库");
            var sb = new StringBuilder();
            sb.Append("SELECT P.PALLET_NO,P.MATER_NO,M.MATER_NAME ");
            sb.Append("FROM TPROC_BIND_PRODUCT P LEFT JOIN PSB_MATERIAL M ON P.MATER_NO = M.MATER_NO ");
            sb.Append("WHERE P.OBJID = '" + this.objidBound.ToString() + "'");
            var dt = dbLocal.Connection.QueryTable(sb.ToString());

            if(dt.Rows.Count > 0)
            {
                palletNo = dt.Rows[0]["PALLET_NO"].ToString();
                materNo = dt.Rows[0]["MATER_NO"].ToString();
                materName = dt.Rows[0]["MATER_NAME"].ToString();
            }

            //2.插入运行信息集合表
            sb = new StringBuilder();
            var param = new DynamicParameters();
            sb.Append("INSERT INTO EMS_RUN_RECORD(CREATETIME,WORK_TYPE,PALLET_NO,MATER_NO,MATER_NAME,RUN_CODE,WORK_DESC) ");
            sb.Append("VALUES(GETDATE(),@WORK_TYPE,@PALLET_NO,@MATER_NO,@MATER_NAME,@RUN_CODE,@WORK_DESC)");
            param.Add("WORK_TYPE", "I");
            param.Add("PALLET_NO", palletNo.ToUpper());
            param.Add("MATER_NO", materNo.ToUpper());
            param.Add("MATER_NAME", materName.ToUpper());
            param.Add("RUN_CODE", errNo);
            param.Add("WORK_DESC", errDesc.ToUpper());

            dbLocal.Connection.Execute(sb.ToString(), param);
        }


        /// <summary>
        /// 绑定结果查询
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        public PalletBound PalletBound(string palletNo)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                var strSql = new StringBuilder();
                var param = new DynamicParameters();
                param.Add("palletNo", palletNo);
                strSql.Append("select T.PALLET_NO, T1.MATER_NO, T.PRODUCT_GUID from PSB_PALLET t left join psb_PRODUCT T1 ON T1.PRODUCT_GUID = T.PRODUCT_GUID WHERE T.PALLET_NO = @palletNo ");

                var dt = dbLocal.Connection.QueryTable(strSql.ToString(), param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return new PalletBound
                    {
                        palletNo = dt.Rows[0]["PALLET_NO"].ToString(),
                        materNo = dt.Rows[0]["MATER_NO"].ToString(),
                        productGuid = dt.Rows[0]["PRODUCT_GUID"].ToString()
                    };

                }
                else
                {
                    return new PalletBound
                    {
                        palletNo = "该工装未绑定信息",
                        materNo = "该工装未绑定信息",
                        productGuid = "该工装未绑定信息"

                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 物料类型列表查询
        /// </summary>
        /// <param name="materNo"></param>
        /// <returns></returns>
        public string[][] MaterTypeListQuery()
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                var strSql = new StringBuilder(); 
                strSql.Append($"select mater_type_no,mater_type_name from psb_material_type ");

                var dt = dbLocal.Connection.QueryTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    string[][] list = new string[dt.Rows.Count + 1][];
                    for(int i = 0;i < dt.Rows.Count;i++)
                    {
                        var typeNo = dt.Rows[i]["mater_type_no"].ToString();
                        var typeName = dt.Rows[i]["mater_type_name"].ToString();

                        list[i] = new string[2];
                        list[i][0] = typeNo;
                        list[i][1] = typeName;
                    }

                    //添加空托盘类型
                    list[dt.Rows.Count] = new string[2];
                    list[dt.Rows.Count][0] = "BPALLET";
                    list[dt.Rows.Count][1] = "空托盘/垛";
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 物料类型列表查询
        /// </summary>
        /// <param name="materNo"></param>
        /// <returns></returns>
        public string[][] PalletTypeListQuery()
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                var strSql = new StringBuilder();
                strSql.Append("select pallet_type_no,pallet_type_name from psb_pallet_type ");

                var dt = dbLocal.Connection.QueryTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    string[][] list = new string[dt.Rows.Count][];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var typeNo = dt.Rows[i]["pallet_type_no"].ToString();
                        var typeName = dt.Rows[i]["pallet_type_name"].ToString();
                        list[i] = new string[2];
                        list[i][0] = typeNo;
                        list[i][1] = typeName;
                    }                
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 物料编号查询
        /// </summary>
        /// <param name="materNo"></param>
        /// <returns></returns>
        public string[][] MaterNoArrQuery(string materType)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                var strSql = new StringBuilder();
                strSql.Append(" select mater_no,mater_name from psb_material ");
                strSql.Append($" where mater_type = '{materType}' ");

                var dt = dbLocal.Connection.QueryTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    string[][] list = new string[dt.Rows.Count][];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var materNo = dt.Rows[i]["mater_no"].ToString();
                        var materName = dt.Rows[i]["mater_name"].ToString();
                        list[i] = new string[2];
                        list[i][0] = materNo;
                        list[i][1] = materName;
                    }
                    return list;                
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 物料回库工位区域查询
        /// </summary>
        /// <param name="materNo"></param>
        /// <returns></returns>
        public Dictionary<string,string> BackLocAreaQuery()
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                var strSql = new StringBuilder();
                strSql.Append("select area_no,area_name from psb_area where area_level = '3'");

                var dt = dbLocal.Connection.QueryTable(strSql.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    Dictionary<string, string> list = new Dictionary<string, string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var areaNo = dt.Rows[i]["area_no"].ToString();
                        var areaName = dt.Rows[i]["area_name"].ToString();
                        list.Add(areaNo,areaName);
                    }
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 物料回库工位区域查询
        /// </summary>
        /// <param name="materNo"></param>
        /// <returns></returns>
        public string LocResidueMaterBack(Bound bound)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                DynamicParameters para = new DynamicParameters();
                para.Add("i_mater_no", bound.MaterNo);
                para.Add("i_sloc_no", bound.LocNo);              
                para.Add("i_pallet_no", bound.PalletNo);
                para.Add("i_product_qty", bound.ProductQty);
                para.Add("i_creation_by", bound.UserName);
                para.Add("o_err_code", null, DbType.String, ParameterDirection.Output, size: 50);
                para.Add("o_err_desc", null, DbType.String, ParameterDirection.Output, size: 50);
                dbLocal.Connection.Execute("proc_1001_store_in_oddments", para, commandType: CommandType.StoredProcedure);
                var errNo = para.Get<string>("o_err_code");
                var errDesc = para.Get<string>("o_err_desc");

                return errDesc;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 指定库位出库，获取任务信息
        /// </summary>
        /// <param name="binNo"></param>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public StorageOutInfo GetOutStorageInfoFromDB(string binNo, string storeNo, string userName)
        {
            try
            {
                if (!chackBinNo(binNo, storeNo))
                {
                    return new StorageOutInfo
                    {
                        TaskNo = "此库位没有货物",
                        PalletNo = "此库位没有货物",
                        BinNo = "此库位没有货物"

                    };
                }
                else
                {
                    var dbLocal = GetDb(storeNo);
                    var strSql = new StringBuilder();
                    var param = new DynamicParameters();
                    strSql.Append("select task_no ,pallet_no,sloc_no from wbs_task where sloc_no = @slocNo");
                    param.Add("slocNo", binNo);
                    if (InsertOrder(storeNo, userName) && InsertOrderLine(binNo, storeNo))
                    {
                        var dt = dbLocal.Connection.QueryTable(strSql.ToString(), param);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            return new StorageOutInfo
                            {
                                TaskNo = dt.Rows[0]["task_no"].ToString(),
                                PalletNo = dt.Rows[0]["pallet_no"].ToString(),
                                BinNo = dt.Rows[0]["sloc_no"].ToString()
                            };
                        }

                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取任务基本信息
        /// </summary>
        /// <param name="binNo"></param>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public StorageOutInfo selectTask(string binNo, string storeNo)
        {
            try
            {
                var dbLocal = GetDb(storeNo);
                var strSql = new StringBuilder();
                var param = new DynamicParameters();
                strSql.Append("select task_no ,pallet_no,sloc_no from wbs_task where sloc_no = @slocNo");
                param.Add("slocNo", binNo);
                var dt = dbLocal.Connection.QueryTable(strSql.ToString(), param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return new StorageOutInfo
                    {
                        TaskNo = dt.Rows[0]["task_no"].ToString(),
                        PalletNo = dt.Rows[0]["pallet_no"].ToString(),
                        BinNo = dt.Rows[0]["sloc_no"].ToString()
                    };
                }
                else
                {
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 指定库位出库，插入订单
        /// </summary>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public bool InsertOrder(string storeNo, string userName)
        {
            try
            {
                string elocN = "600202";
                var dbLocal = GetDb(storeNo);
                DateTime d = DateTime.Now;
                this.orderNo = d.ToString("yyyyMMddHHmmss");
                var strSql = new StringBuilder();
                var param = new DynamicParameters();
                strSql.Append("insert into wbs_order (order_no,order_type_no,order_type_module,created_by,source_type,eloc_no,user_name) values(@orderNo,'100030','O','3',3,@elocNo,@username)");
                param.Add("orderNo", this.orderNo);
                param.Add("elocNo", elocN);
                param.Add("username", userName);
                var dt = dbLocal.Connection.Execute(strSql.ToString(), param);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 指定库位出库，插入行项目
        /// </summary>
        /// <param name="binNo"></param>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public bool InsertOrderLine(string binNo, string storeNo)
        {
            try
            {
                string elocN = "600202";
                var dbLocal = GetDb(storeNo);
                System.Guid guid = System.Guid.NewGuid(); //Guid 类型                
                string strGUID = System.Guid.NewGuid().ToString(); //直接返回字符串类
                var strSql = new StringBuilder();
                var param = new DynamicParameters();
                strSql.Append("insert into wbs_order_line (order_line_guid, order_no,sort_id,sloc_no,eloc_no,limit_bin_no)values(@orderLineGuid,@orderNo,1,@slocNo,@elocNo,@limitBinNo)");
                param.Add("orderLineGuid", strGUID);
                param.Add("orderNo", this.orderNo);
                param.Add("slocNo", binNo);
                param.Add("elocNo", elocN);
                param.Add("limitBinNo", binNo);
                var dt = dbLocal.Connection.Execute(strSql.ToString(), param);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 查找相应库位是否为空
        /// </summary>
        public bool chackBinNo(string binNo, string storeNo)
        {
            try
            {
                var dbLocal = GetDb(storeNo);
                var strSql = new StringBuilder();
                var param = new DynamicParameters();
                strSql.Append("SELECT * FROM WBS_BIN WHERE BIN_NO = @BINNO");
                param.Add("BINNO", binNo);
                var dt = dbLocal.Connection.QueryTable(strSql.ToString(), param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 查询托盘 库区，物料名称，托盘重量
        /// </summary>
        /// <param name="palletNo"></param>
        /// <returns></returns>
        public QuaryPallet quaryPallet(string palletNo)
        {
            try
            {
                string strSql = "SELECT T1.WH_NO,T2.MATER_NAME,T.PALLET_WEIGHT FROM X_PALLET T JOIN X_PRODUCT T1 ON T.PALLET_NO = T1.PALLET_NO JOIN X_MATERIAL T2 ON T1.MATER_NO = T2.MATER_NO WHERE T.PRODUCT_GUID = T1.PRODUCT_GUID AND T.PALLET_NO = @PALLETNO";
                var dt = db.Connection.QueryTable(strSql, new { PALLETNO = palletNo });
                if (dt != null && dt.Rows.Count > 0)
                {
                    return new QuaryPallet()
                    {
                        WhNo = (dt.Rows[0]["WH_NO"] ?? "").ToString(),
                        MaterialNo = (dt.Rows[0]["MATER_NAME"] ?? "").ToString(),
                        ProductWeight = (dt.Rows[0]["PALLET_WEIGHT"] ?? "").ToString()
                    };
                }
                else
                {
                    return new QuaryPallet()
                    {
                        WhNo = "未绑定",
                        MaterialNo = "未绑定",
                        ProductWeight = "未绑定"
                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取订单基本信息
        /// </summary>
        /// <returns></returns>
        public string[] GetOrderNoFromDB()
        {
            try
            {
                string strSql = "select t.order_no,t.ORDER_STATUS,t.ORDER_TYPE_NO,t.CUSTOMER_NAME from wbs_order t";
                var iList = db.Connection.Query<OrderInfo>(strSql);
                int len = iList.Count();
                string[] orderNoList = new string[len];
                int i = 0;
                foreach (var itm in iList)
                {
                    orderNoList[i++] = itm.ORDER_NO;
                }
                return orderNoList;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 调用发货存储过程
        /// </summary>
        /// <param name="palletNo"></param>
        /// <param name="barcodeNo"></param>
        /// <returns></returns>
        public string[] SendGoodsForPalletDBMethod(string palletNo, string barcodeNo, string userName)
        {
            string[] result = new string[3];
            try
            {
                DynamicParameters para = new DynamicParameters();
                para.Add("PALLET_NO", palletNo);
                para.Add("PRODUCT_NO", barcodeNo);
                para.Add("USERS_NAME", userName);

                para.Add("O_ERR_DESC", null, DbType.String, ParameterDirection.Output, size: 50);
                para.Add("O_MATERIAL", null, DbType.String, ParameterDirection.Output, size: 50);
                para.Add("O_SENDNUMBER", null, DbType.String, ParameterDirection.Output, size: 50);
                db.Connection.Execute("PROC_0030_SENDING", para, commandType: CommandType.StoredProcedure);
                var errDesc = para.Get<string>("O_ERR_DESC");
                var material = para.Get<string>("O_MATERIAL");
                var sendnumber = para.Get<String>("O_SENDNUMBER");
                result[0] = errDesc;
                result[1] = material;
                result[2] = sendnumber;
                return result;
            }
            catch (Exception ex)
            {
                result[0] = ex.Message.ToString();
                this.objidPalletInWare = "";
                return result;
            }
        }

        /// <summary>
        /// 拉动空工装出库，获取任务信息
        /// </summary>
        /// <param name="locNo"></param>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public BpalletOutInfo GetOutBpalletInfoFromDB(string binNo, string storeNo, string userName, string locNo)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                var strSql = new StringBuilder();
                var param = new DynamicParameters();
                if (InsertBpalletOrder(locNo, storeNo, userName) && InsertBpalletOrderLine(locNo, storeNo))
                {
                    //strSql.Append("SELECT T1.TASK_NO,T1.PALLET_NO,T1.SLOC_NO FROM WBS_ORDER_LINE T  JOIN WBS_TASK T1 ON T.ORDER_LINE_GUID = T1.ORDER_LINE_GUID WHERE T.ORDER_NO = @orderNo ");
                    //param.Add("orderNo", this.orderNo);
                    //var dt = dbLocal.Connection.QueryTable(strSql.ToString(), param);
                    //if (dt != null && dt.Rows.Count > 0)
                    //{
                    //    return new BpalletOutInfo
                    //    {
                    //        TaskNo = dt.Rows[0]["task_no"].ToString(),
                    //        PalletNo = dt.Rows[0]["pallet_no"].ToString(),
                    //        BinNo = dt.Rows[0]["sloc_no"].ToString()
                    //    };
                    //}
                    //else
                    //{
                    //    return new BpalletOutInfo
                    //    {
                    //        TaskNo = "未生成任务",
                    //        PalletNo = "未生成任务",
                    //        BinNo = "未生成任务"
                    //    };
                    //}
                    return new BpalletOutInfo
                    {
                        TaskNo = "创建出库订单成功",
                        PalletNo = "创建出库订单成功",
                        BinNo = "创建出库订单成功"
                    };

                }
                else
                    {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 空托盘出库，插入订单
        /// </summary>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public bool InsertBpalletOrder(string locNo, string storeNo, string userName)
        {
            try
            {
                string elocN = locNo;
                var dbLocal = GetDb("原材料库");
                DateTime d = DateTime.Now;
                this.orderNo = d.ToString("yyyyMMddHHmmss");
                var strSql = new StringBuilder();
                var param = new DynamicParameters();                
                param.Add("orderNo", this.orderNo);
                param.Add("elocNo", elocN);
                param.Add("username", userName);
                strSql.Append("insert into wbs_order (order_no,order_type_no,order_type_module,created_by,source_type,eloc_no) values(@orderNo,'100030','O',@username,3,(select LOC_NO from PSB_LOC where LOC_PLC_NO = @elocNo))");
                var dt = dbLocal.Connection.Execute(strSql.ToString(), param);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 空托盘出库，插入行项目
        /// </summary>
        /// <param name="locNo"></param>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public bool InsertBpalletOrderLine(string locNo, string storeNo)
        {
            try
            {
                string elocN = locNo;
                var dbLocal = GetDb("原材料库");
                System.Guid guid = System.Guid.NewGuid(); //Guid 类型                
                string strGUID = System.Guid.NewGuid().ToString(); //直接返回字符串类
                var strSql = new StringBuilder();
                var param = new DynamicParameters();
                param.Add("orderLineGuid", strGUID);
                param.Add("orderNo", this.orderNo);
                param.Add("elocNo", elocN);
                strSql.Append("insert into wbs_order_line (order_line_guid, order_no,sort_id,line_status,material_no,eloc_no)values(@orderLineGuid,@orderNo,1,0,'BPALLET',(select LOC_NO from PSB_LOC where LOC_PLC_NO = @elocNo))");
                var dt = dbLocal.Connection.Execute(strSql.ToString(), param);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 入库信息异常查询
        /// </summary>
        /// <param name="locNo"></param>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        public TaskRequestErr TaskRequesterr(string storeNo, string locNo, string palletNo, string errDesc)
        {
            try
            {
                var dbLocal = GetDb(storeNo);
                var strSql = new StringBuilder();
                var param = new DynamicParameters();
                param.Add("palletNo", palletNo);
                param.Add("slocNo", locNo);
                strSql.Append("SELECT T.PALLET_NO,T.ERR_DESC,T.SLOC_NO FROM TPROC_0100_TASK_REQUEST T left join PSB_LOC t1 on t1.LOC_NO = t.SLOC_NO WHERE T.PALLET_NO = @palletNo AND T1.LOC_PLC_NO = @slocNo");

                var dt = dbLocal.Connection.QueryTable(strSql.ToString(), param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return new TaskRequestErr
                    {
                        PalletNo = dt.Rows[0]["PALLET_NO"].ToString(),
                        LocNo = dt.Rows[0]["SLOC_NO"].ToString(),
                        ErrDesc = dt.Rows[0]["ERR_DESC"].ToString()
                    };

                }
                else
                {
                    return new TaskRequestErr
                    {
                        PalletNo = "未请求任务",
                        LocNo = "未请求任务",
                        ErrDesc = "未请求任务"

                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 码垛机站台更新批次信息
        /// </summary>
        /// <param name="locNo"></param>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        private bool LocBatchNoUpdate(string locNo,string batchNo)
        {
            try
            {
                var strSql = new StringBuilder();
                strSql.Append($"UPDATE PEM_LOC_STATUS SET BATCH_NO = '{batchNo}'");
                strSql.Append($"WHERE LOC_NO = '{locNo}'");

                var dbLocal = GetDb("原材料库");
                dbLocal.Connection.Execute(strSql.ToString());
                return true;
            }catch(Exception e)
            {
                return false;
            }         
        }

        /// <summary>
        /// AGV指令创建
        /// </summary>
        /// <param name="locNo"></param>
        /// <returns></returns>
        private bool AgvCmdCreate(string palletType)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                var objid = dbLocal.Connection.ExecuteScalar<int>("SELECT NEXT VALUE FOR SEQ_TPROC_0200_CMD_REQUEST");

                string slocNo = "";
                string elocNo = "";
                //确定起止站台
                switch(palletType)
                {
                    case "1000":
                        slocNo = "300101";
                        elocNo = "600101";
                        break;
                    case "1001":
                        slocNo = "300102";
                        elocNo = "600102";
                        break;
                    case "2000":
                        slocNo = "600201";
                        elocNo = "300201";
                        break;
                    case "2001":
                        slocNo = "600202";
                        elocNo = "300202";
                        break;
                }


                var sb = new StringBuilder();
                sb.Append(" INSERT INTO TPROC_0200_CMD_REQUEST");
                sb.Append(" (OBJID, CURR_LOC_NO,ELOC_NO)");
                sb.Append(" VALUES");
                sb.Append(" (@OBJID, @SLOCNO,@ELOCNO)");
                var param = new DynamicParameters();
                param.Add("OBJID", objid);
                param.Add("SLOCNO", slocNo);
                param.Add("ELOCNO", elocNo);

                dbLocal.Connection.Execute(sb.ToString(), param);

                var dp = new DynamicParameters();
                dp.Add("I_PARAM_OBJID", objid);
                dp.Add("O_CMD_OBJID", 0, DbType.Int32, ParameterDirection.Output);
                dp.Add("O_ERR_CODE", 0, DbType.Int32, ParameterDirection.Output);
                dp.Add("O_ERR_DESC", 0, DbType.String, ParameterDirection.Output, size: 80);
                dbLocal.Connection.Execute("PROC_0503_AGV_CMD_REQUEST", param: dp, commandType: CommandType.StoredProcedure);
                var errMsg = dp.Get<string>("O_ERR_DESC");
                return string.IsNullOrEmpty(errMsg) ? true : false;
            }
            catch(Exception e)
            {
                return false;
            }           
        }   

        private bool deleteTask(int taskNo)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                string strSql = $"delete from wbs_task where TASK_NO = {taskNo}";
                dbLocal.Connection.Execute(strSql);
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 入库请求,插入工装号到pem_loc_status表
        /// </summary>
        /// <param name="locNo"></param>
        /// <param name="palletNo"></param>
        /// <param name="storeNo"></param>
        /// <returns></returns>
        //public bool palletUpdate(string locNo, string palletNo)
        //{
        //    try
        //    {
        //        var dbLocal = GetDb("原材料库");
        //        var strSql = new StringBuilder();
        //        var param = new DynamicParameters();
        //        strSql.Append("UPDATE PEM_LOC_STATUS SET SCAN_RFID_NO = @palletNo WHERE LOC_NO = @locNo ");
        //        param.Add("palletNo", palletNo);
        //        param.Add("locNo", locNo);
        //        var i = dbLocal.Connection.Execute(strSql.ToString(), param);
        //        if (i > 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        /// 获取任务objid
        /// </summary>
        public int GetObjidForRequestTask()
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                return dbLocal.Connection.ExecuteScalar<int>("select next value for [dbo].[SEQ_TPROC_0100_TASK_REQUEST]");
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 请求生成任务
        /// </summary>
        public int RequestTask(long objid, string slocNo, string orderTypeNo, string palletNo,ref string errMsg,ref int errMsgNo)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                var dt = dbLocal.Connection.QueryTable($"SELECT * FROM TPROC_0100_TASK_REQUEST T WHERE T.OBJID = {objid}");
                if (dt == null || dt.Rows.Count == 0)
                {
                    var sb = new StringBuilder();
                    sb.Append(" INSERT INTO TPROC_0100_TASK_REQUEST");
                    sb.Append(" (OBJID, ORDER_TYPE_NO, SLOC_NO, PALLET_NO)");
                    sb.Append(" VALUES ");
                    sb.Append(" (@OBJID, @ORDERTYPENO ,@SLOCNO, @PALLETNO)");
                    var param = new DynamicParameters();
                    param.Add("OBJID", objid);
                    param.Add("SLOCNO", slocNo);
                    param.Add("PALLETNO", palletNo);
                    param.Add("ORDERTYPENO", orderTypeNo);
                    dbLocal.Connection.Execute(sb.ToString(), param);
                }
                else
                {
                    var sb = new StringBuilder();
                    sb.Append(" UPDATE TPROC_0100_TASK_REQUEST");
                    sb.Append(" SET ORDER_TYPE_NO = @ORDERTYPENO");
                    sb.Append(" ,SLOC_NO = @SLOCNO");
                    sb.Append(" ,PALLET_NO = @PALLETNO");
                    sb.Append(" ,PRODUCT_WEIGHT = @PRODUCTWEIGHT");
                    sb.Append(" WHERE OBJID = @OBJID");
                    var param = new DynamicParameters();
                    param.Add("OBJID", objid);
                    param.Add("SLOCNO", slocNo);
                    param.Add("PALLETNO", palletNo);
                    param.Add("ORDERTYPENO", orderTypeNo);
                    dbLocal.Connection.Execute(sb.ToString(), param);
                }
                //执行存储过程
                var dp = new DynamicParameters();
                dp.Add("I_PARAM_OBJID", objid);
                dp.Add("O_TASK_NO", 0, DbType.Int32, ParameterDirection.Output);
                dp.Add("O_ERR_CODE", 0, DbType.Int32, ParameterDirection.Output);
                dp.Add("O_ERR_DESC", 0, DbType.String, ParameterDirection.Output, size: 80);
                dbLocal.Connection.Execute("PROC_0100_TASK_REQUEST", param: dp, commandType: CommandType.StoredProcedure);
                errMsg = dp.Get<string>("O_ERR_DESC");
                errMsgNo = dp.Get<int>("O_ERR_CODE");
                return dp.Get<int>("O_TASK_NO");
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return -1;
            }
        }
        /// <summary>
        /// 获取指令objid
        /// </summary>
        /// <returns></returns>
        public int GetObjidForRequestCmd()
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                return dbLocal.Connection.ExecuteScalar<int>("select next value for SEQ_TPROC_0200_CMD_REQUEST"); 
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 请求生成指令
        /// </summary>
        public string RequstCmd(int taskNo,string locNo,ref string errMsgDesc,ref int errMsgNo)
        {
            var dbLocal = GetDb("原材料库");
            int equestCmdObjid;
            //获取指令生成请求OBJID
            equestCmdObjid = this.GetObjidForRequestCmd();
            if (equestCmdObjid<= 0)
            {
                return "获取指令objid失败";
            }
            string errMsgD = string.Empty;
            int errMsgN = 0;
            //传入参数，请求指令
            var CmdId = this.RequestCmd(equestCmdObjid, locNo, taskNo, ref errMsgD, ref errMsgN);
            if (CmdId <= 0)
            {
                errMsgDesc = errMsgD;
                errMsgNo = errMsgN;
                return "请求生成指令失败";
            }

            return "请求生成指令成功";
        }
        /// <summary>
        /// 请求生成指令
        /// </summary>
        public int RequestCmd(int objid, string slocNo, int taskNo, ref string errMsgDesc,ref int errMsgNo)
        {
            try
            {
                var dbLocal = GetDb("原材料库");
                //获取是否已经插入数据
                var dt = dbLocal.Connection.QueryTable($"SELECT * FROM TPROC_0200_CMD_REQUEST T WHERE T.OBJID = {objid}");
                if (dt == null || dt.Rows.Count == 0)
                {
                    var sb = new StringBuilder();
                    sb.Append(" INSERT INTO TPROC_0200_CMD_REQUEST");
                    sb.Append(" (OBJID, TASK_NO, CURR_LOC_NO)");
                    sb.Append(" VALUES");
                    sb.Append(" (@OBJID, @TASKNO, @LOCNO)");
                    var param = new DynamicParameters();
                    param.Add("OBJID", objid);
                    param.Add("TASKNO", taskNo);
                    param.Add("LOCNO", slocNo);
                    dbLocal.Connection.Execute(sb.ToString(), param);
                }
                //执行存储过程
                var dp = new DynamicParameters();
                dp.Add("I_PARAM_OBJID", objid);
                dp.Add("O_CMD_OBJID", 0, DbType.Int32, ParameterDirection.Output);
                dp.Add("O_ERR_CODE", 0, DbType.Int32, ParameterDirection.Output);
                dp.Add("O_ERR_DESC", 0, DbType.String, ParameterDirection.Output, size: 80);
                dbLocal.Connection.Execute("PROC_0200_CMD_REQUEST", param: dp, commandType: CommandType.StoredProcedure);
                errMsgDesc = dp.Get<string>("O_ERR_DESC");
                errMsgNo = dp.Get<int>("O_ERR_CODE");
                return dp.Get<int>("O_CMD_OBJID");
            }
            catch (Exception ex)
            {
                errMsgDesc = ex.Message;
                errMsgNo = 0;
                return -1;
            }
        }
        /// <summary>
        /// 获取指令信息
        /// </summary>
        public string SelectTaskCmd(int taskNo)
        {
            try
            {
                var dbLocal = GetDb("原材料库") ;
                var strSql = new StringBuilder();
                var param = new DynamicParameters();
                strSql.Append("select t.ELOC_PLC_NO from WBS_TASK_CMD t where t.TASK_NO = @TASKNO");
                param.Add("TASKNO", taskNo);
                var dt = dbLocal.Connection.QueryTable(strSql.ToString(), param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["ELOC_PLC_NO"].ToString();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


    }


}