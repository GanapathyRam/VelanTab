﻿using Libraries.Entity;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

namespace VV
{
    public class DBUtil
    {
        SqlConnection conn;

        /// <summary>
        /// Initialises the SQL Connection & also opens the Connection
        /// </summary>
        private void init()
        {
            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["VVConnection"].ToString());
                conn.Open();
            }
            catch (Exception ex)
            {
                Logger.Write(this.GetType().ToString() + " : init() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
            }
        }

        /// <summary>
        /// Checks if the User is a validUser or not
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool IsValidUser(String UserName, String Password)
        {
            bool isValidUser = false;
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("spGetLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@UserName", UserName));
                cmd.Parameters.Add(new SqlParameter("@Password", Password));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0][0].ToString().Trim().Equals("1"))
                            isValidUser = false;
                        else
                            isValidUser = true;
                    }
                }

                conn.Close();
                return isValidUser;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : IsValidUser : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        /// <summary>
        /// Gets the list of accessible screens for this User
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public DataSet GetScreenAccessInfo(String UserName)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("spGetScreenAccessInfo", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@UserName", UserName));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetScreenAccessInfo : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        /// <summary>
        /// Does a bulk ingestion of the BaaN data into the Database
        /// </summary>
        /// <param name="ds"></param>
        public void BulkIngestIntoDatabase(DataSet ds)
        {
            try
            {
                this.init();

                using (SqlBulkCopy copy = new SqlBulkCopy(conn))
                {
                    copy.ColumnMappings.Add(0, "OrderType");
                    copy.ColumnMappings.Add(1, "OrderNo");
                    copy.ColumnMappings.Add(2, "CustomerName");
                    copy.ColumnMappings.Add(3, "CustomerOrderNo");
                    copy.ColumnMappings.Add(4, "LineNum");
                    copy.ColumnMappings.Add(5, "Pos");
                    copy.ColumnMappings.Add(6, "Item");
                    copy.ColumnMappings.Add(7, "Description");
                    copy.ColumnMappings.Add(8, "ItemGroup");
                    copy.ColumnMappings.Add(9, "SalesOrderRevision");
                    copy.ColumnMappings.Add(10, "RevisionDate");
                    copy.ColumnMappings.Add(11, "Area");
                    copy.ColumnMappings.Add(12, "OrderDate");
                    copy.ColumnMappings.Add(13, "OriginalPromDate");
                    copy.ColumnMappings.Add(14, "PlannedDelDate");
                    copy.ColumnMappings.Add(15, "OrderQty");
                    copy.ColumnMappings.Add(16, "BalanceQty");
                    copy.ColumnMappings.Add(17, "Amount");
                    copy.ColumnMappings.Add(18, "InvoicedQty");
                    copy.ColumnMappings.Add(19, "FGQty");
                    copy.ColumnMappings.Add(20, "WIPQty");
                    copy.ColumnMappings.Add(21, "RelDate");
                    copy.ColumnMappings.Add(22, "ProdCompletionDate");
                    copy.ColumnMappings.Add(23, "ProdRemarks");
                    copy.ColumnMappings.Add(24, "PlanWeek");
                    copy.ColumnMappings.Add(25, "WIPWeek");
                    copy.ColumnMappings.Add(26, "FGWeek");
                    copy.ColumnMappings.Add(27, "ToReleaseQty");

                    copy.DestinationTableName = "MISOrderStatus";
                    copy.WriteToServer(ds.Tables[0]);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : BulkIngestIntoDatabase : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }
        

        /// <summary>
        /// Get all the records from the ItemGroupMapping
        /// </summary>
        /// <returns></returns>
        public DataSet GetItemGroupDataMapping()
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("spGetItemGroupMapping", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetItemGroupDataMapping() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }
      

        #region Tab

        public DataSet GetICSOrdersDetails(string ProdOrderNo)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetGridICSOrderDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : spGetICSOrderDetails : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetNonICSOrdersDetails(string ProdOrderNo)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetGridNonICSOrderDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : spGetNonICSOrderDetails : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void UpdateNonICSOrdersDetails(string ProductionOrderNo, string SerialNo, string BodyHeatNo, string BodyRTNo,string BonnetHeatNo, string BonnetRTNo,
            string WedgeHeatNo, string WedgeRTNo, string PipeHeatNo, string PipeRTNo,string Bonnet2HeatNo, string Bonnet2RTNo, string ActuatorSerialNo)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spUpdateNonICSOrderDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserInfo = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@ProductionOrderNo", ProductionOrderNo));
                cmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));
                cmd.Parameters.Add(new SqlParameter("@BodyHeatNo", BodyHeatNo));
                cmd.Parameters.Add(new SqlParameter("@BodyRTNo", BodyRTNo));
                cmd.Parameters.Add(new SqlParameter("@BonnetHeatNo", BonnetHeatNo));
                cmd.Parameters.Add(new SqlParameter("@BonnetRTNo", BonnetRTNo));
                cmd.Parameters.Add(new SqlParameter("@WedgeHeatNo", WedgeHeatNo));
                cmd.Parameters.Add(new SqlParameter("@WedgeRTNo", WedgeRTNo));
                cmd.Parameters.Add(new SqlParameter("@PipeHeatNo", PipeHeatNo));
                cmd.Parameters.Add(new SqlParameter("@PipeRTNo", PipeRTNo));
                cmd.Parameters.Add(new SqlParameter("@Bonnet2HeatNo", Bonnet2HeatNo));
                cmd.Parameters.Add(new SqlParameter("@Bonnet2RTNo", Bonnet2RTNo));
                cmd.Parameters.Add(new SqlParameter("@ActuatorSerialNo", ActuatorSerialNo));

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : UpdateNonICSOrdersDetails() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void UpdateICSOrdersDetails(string ProdOrderNo, string BodyHeatNo, string BodyRTNo, string BonnetHeatNo, string BonnetRTNo,
           string WedgeHeatNo, string WedgeRTNo, string PipeHeatNo, string PipeRTNo, string Bonnet2HeatNo, string Bonnet2RTNo, string ActuatorSerialNo)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spUpdateICSOrderDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserInfo = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));
                cmd.Parameters.Add(new SqlParameter("@BodyHeatNo", BodyHeatNo));
                cmd.Parameters.Add(new SqlParameter("@BodyRTNo", BodyRTNo));
                cmd.Parameters.Add(new SqlParameter("@BonnetHeatNo", BonnetHeatNo));
                cmd.Parameters.Add(new SqlParameter("@BonnetRTNo", BonnetRTNo));
                cmd.Parameters.Add(new SqlParameter("@WedgeHeatNo", WedgeHeatNo));
                cmd.Parameters.Add(new SqlParameter("@WedgeRTNo", WedgeRTNo));
                cmd.Parameters.Add(new SqlParameter("@PipeHeatNo", PipeHeatNo));
                cmd.Parameters.Add(new SqlParameter("@PipeRTNo", PipeRTNo));
                cmd.Parameters.Add(new SqlParameter("@Bonnet2HeatNo", Bonnet2HeatNo));
                cmd.Parameters.Add(new SqlParameter("@Bonnet2RTNo", Bonnet2RTNo));
                cmd.Parameters.Add(new SqlParameter("@ActuatorSerialNo", ActuatorSerialNo));

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : UpdatePassword() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void UpdateBulkUpdateBodyBonnetHeatNo(string ProdOrderNo, string SerialNo, string BodyHeatNo, string BonnetHeatNo)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spUpdateForBulkUpdateBonnetBodyHeatNo]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserInfo = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@ProductionOrderNo", ProdOrderNo));
                cmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));
                cmd.Parameters.Add(new SqlParameter("@BodyHeatNo", BodyHeatNo));
                cmd.Parameters.Add(new SqlParameter("@BonnetHeatNo", BonnetHeatNo));

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : UpdateBulkUpdateBodyBonnetHeatNo : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        #endregion

        #region Patrol Inspection

        public DataSet GetLocationMaster()
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select LocationCode, LocationName from IPLocationMaster", conn);
                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetLocationMaster : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetSubLocationMaster()
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select SubLocationCode, SubLocationName from IPSubLocationMaster", conn);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetSubLocationMaster : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public string GetPatrolNumber()
        {
            DataSet ds = new DataSet();
            var patrolNumber = string.Empty;

            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[uspGetPatrolNumber]", conn);
                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);
                conn.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    patrolNumber = Convert.ToString(ds.Tables[0].Rows[0]["PatrolNumber"].ToString());
                }

                return patrolNumber;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetPatrolNumber : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public bool IsPatrolNumberExist(string PatrolNumber)
        {
            DataSet ds = new DataSet();
            var patrolNumber = string.Empty;
            bool isExist = false;

            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select PatrolNumber from PatrolMaster where PatrolNumber = '"+ PatrolNumber + "'", conn);
                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);
                conn.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    patrolNumber = Convert.ToString(ds.Tables[0].Rows[0]["PatrolNumber"].ToString());

                    if (!string.IsNullOrEmpty(patrolNumber))
                    {
                        isExist = true;
                    }
                }

                return isExist;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetPatrolNumber : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetMeetMaster()
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select MeetCode, MeetName from MeetsMaster", conn);
                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetMeetMaster : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }


        public DataSet GetProductionReleaseNewForPatrolWithSerial(String ProdOrderNo, String LocationCode, String SubLocationCode)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetProductionReleaseNewForPatrolWithSerial]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));
                cmd.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));
                cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetProductionReleaseNewForPatrolWithSerial : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetMISOrderStatusForPatrol(String ProdOrderNo)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetMISOrderStatusForPatrol]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetMISOrderStatusForPatrol() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetProductionReleaseNewForPatrolWithOutSerial(String ProdOrderNo, String LocationCode, String SubLocationCode)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetProductionReleaseNewForPatrolWithOutSerial]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));
                cmd.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));
                cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : spGetProductionReleaseNewForPatrolWithOutSerial : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetProductionReleaseNewForPatrol(String ProdOrderNo, String LocationCode, String SubLocationCode)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetProductionReleaseNewForPatrol]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));
                cmd.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));
                cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));


                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : spGetProductionReleaseNewForPatrol : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet RetriveByEmployeesNameForPatrol()
        {
            DataSet ds = new DataSet();

            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select EmployeeCode, EmployeeName from DCEmployeeMaster where Qc = 1 Order by DCEmployeeMaster.EmployeeName", conn);
                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();
                return ds;

            }
            catch (Exception ex)
            {
                //LogError(ex, "Exception from export button click!");
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : RetriveByEmployeesNameForPatrol() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetShiftMasterDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select ShiftCode, ShiftName from ShiftMaster Order by ShiftName desc", conn);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetShiftMasterDetails : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetOperatorMasterDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select OperatorCode, OperatorName from OperatorMaster Order by OperatorName", conn);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetOperatorMasterDetails  : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetWIPAllFromProdOrderNo(string ProdOrder)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select * from WIPall where ProdOrder ='"+ ProdOrder + "'", conn);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetWIPAllFromProdOrderNo  : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetCheckListMasterDetails(String LocationCode, String SubLocationCode)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetCheckListMasterForPatrol]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));
                cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetCheckListMasterDetails : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetPatrolDetails(String PatrolNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetPatrolDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PatrolNumber", PatrolNumber));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetPatrolDetails() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetPatrolInspectionSerial(string PatrolNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select SerialNo from PatrolSerial where PatrolNumber = '"+ PatrolNumber + "'", conn);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetPatrolInspectionSerial() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void InsertPatrolInspectionMaster(String PatrolNumber, DateTime PatrolDate, String LocationCode, String SubLocationCode, String ProdOrderNo,
            int PatrolQty, String OperatorCode, String ShiftCode, int EmployeeCode, String Remarks, DateTime CreatedDateTime)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spInsertPatrolMaster]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@PatrolNumber", PatrolNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@PatrolDate", PatrolDate));

                cmd.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));
                cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));
                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));

                cmd.Parameters.Add(new SqlParameter("@PatrolQty", PatrolQty));
                cmd.Parameters.Add(new SqlParameter("@OperatorCode", OperatorCode));

                cmd.Parameters.Add(new SqlParameter("@ShiftCode", ShiftCode));
                cmd.Parameters.Add(new SqlParameter("@EmployeeCode", EmployeeCode));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));

                cmd.Parameters.Add(new SqlParameter("@CreatedDateTime", CreatedDateTime));


                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : InsertPatrolInspectionMaster() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void UpdatePatrolInspectionMaster(String PatrolNumber, DateTime PatrolDate,  int PatrolQty, String OperatorCode, String ShiftCode, int EmployeeCode, 
            String Remarks, DateTime UpdatedDateTime)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spUpdatePatrolMaster]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@PatrolNumber", PatrolNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@PatrolDate", PatrolDate));

                cmd.Parameters.Add(new SqlParameter("@PatrolQty", PatrolQty));
                cmd.Parameters.Add(new SqlParameter("@OperatorCode", OperatorCode));

                cmd.Parameters.Add(new SqlParameter("@ShiftCode", ShiftCode));
                cmd.Parameters.Add(new SqlParameter("@EmployeeCode", EmployeeCode));
                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));

                cmd.Parameters.Add(new SqlParameter("@UpdatedDateTime", UpdatedDateTime));

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : UpdatePatrolInspectionMaster() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void InsertPatrolInspectionDetails(String PatrolNumber, String CheckListSerial, String Meets, String MeetsComments,
            String Meets1, String Meets1Comments, String MeetsImage, String PatrolImage)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spInsertPatrolDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@PatrolNumber", PatrolNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@CheckListSerial", CheckListSerial));

                cmd.Parameters.Add(new SqlParameter("@Meets", Meets));
                cmd.Parameters.Add(new SqlParameter("@MeetsComments", MeetsComments));

                cmd.Parameters.Add(new SqlParameter("@Meets1", Meets1));
                cmd.Parameters.Add(new SqlParameter("@Meets1Comments", Meets1Comments));

                cmd.Parameters.Add(new SqlParameter("@MeetsImage", MeetsImage));
                cmd.Parameters.Add(new SqlParameter("@PatrolImage", PatrolImage));


                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : InsertPatrolInspectionDetails() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void UpdatePatrolInspectionDetails(String PatrolNumber, String CheckListSerial, String Meets, String MeetsComments,
            String Meets1, String Meets1Comments, String MeetsImage, String PatrolImage)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spUpdatePatrolDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@PatrolNumber", PatrolNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@CheckListSerial", CheckListSerial));

                cmd.Parameters.Add(new SqlParameter("@Meets", Meets));
                cmd.Parameters.Add(new SqlParameter("@MeetsComments", MeetsComments));

                cmd.Parameters.Add(new SqlParameter("@MeetsImage", MeetsImage));
                cmd.Parameters.Add(new SqlParameter("@PatrolImage", PatrolImage));


                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : UpdatePatrolInspectionDetails() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void InsertPatrolInspectionSerials(String PatrolNumber, String SerialNo)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spInsertPatrolSerial]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@PatrolNumber", PatrolNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : InsertPatrolInspectionSerials() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void DeletePatrolInspectionSerials(String PatrolNumber, String SerialNo, int SerialCount, int IsDeleteOrUpdate)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spDeletePatrolSerial]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@PatrolNumber", PatrolNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));
                cmd.Parameters.Add(new SqlParameter("@PatrolQty", SerialCount));
                cmd.Parameters.Add(new SqlParameter("@IsDeleteOrUpdate", IsDeleteOrUpdate));

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : DeletePatrolInspectionSerials() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void DeletePatrolInspection(String PatrolNumber)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spDeletePatrolInspection]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@PatrolNumber", PatrolNumber)); // String

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : DeletePatrolInspection() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetPatrolMasterDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("SELECT Meets, MeetsComments, MeetsImage, PatrolImage FROM PatrolDetail ORDER BY PatrolNumber", conn);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetPatrolMasterDetails  : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetCheckListDetailsForPatrol()
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetCheckListDetailsForPatrol]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetCheckListDetailsForPatrol : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetPatrolMaster(String PatrolNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetPatrolMaster]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PatrolNumber", PatrolNumber));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetPatrolMaster() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetPatrolNumberFromProdOrderNo(string ProdOrderNo, string LocationCode)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetPatrolNumberFromProdOrderNo]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));
                cmd.Parameters.Add(new SqlParameter("@LocationCode", LocationCode));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetPatrolNumberFromProdOrderNo() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        #endregion

        #region Leak Test

        public string GetLeakTestNumber()
        {
            DataSet ds = new DataSet();
            var patrolNumber = string.Empty;

            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[uspGetLeakTestNumber]", conn);
                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);
                conn.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    patrolNumber = Convert.ToString(ds.Tables[0].Rows[0]["LeakTestNumber"].ToString());
                }

                return patrolNumber;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetLeakTestNumber : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetProductionReleaseNewForLeakTestWithSerial(String ProdOrderNo, String SubLocationCode)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetProductionReleaseNewForLeakTestWithSerial]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));
                cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetProductionReleaseNewForLeakTestWithSerial : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetProductionReleaseNewForLeakTestWithOutSerial(String ProdOrderNo, String SubLocationCode)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetProductionReleaseNewForLeakTestWithOutSerial]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));
                cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetProductionReleaseNewForLeakTestWithOutSerial : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetProductionReleaseNewForLeakTest(String ProdOrderNo, String SubLocationCode)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetProductionReleaseNewForLeakTest]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));
                cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));


                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetProductionReleaseNewForLeakTest : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void DeleteLeakTestSerials(String PatrolNumber, String SerialNo, int SerialCount, int IsDeleteOrUpdate)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spDeleteLeakTestSerial]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@LeakTestNumber", PatrolNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));
                cmd.Parameters.Add(new SqlParameter("@LeakTestQty", SerialCount));
                cmd.Parameters.Add(new SqlParameter("@IsDeleteOrUpdate", IsDeleteOrUpdate));

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : DeleteLeakTestSerials() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetLeakTestDetails(String LeakTestNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetLeakTestDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@LeakTestNumber", LeakTestNumber));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetLeakTestDetails() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public bool IsLeakTestNumberExist(string LeakTestNo)
        {
            DataSet ds = new DataSet();
            var patrolNumber = string.Empty;
            bool isExist = false;

            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select LeakTestNumber from LeakTestMaster where LeakTestNumber = '" + LeakTestNo + "'", conn);
                cmd.CommandTimeout = 1000;
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);
                conn.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    patrolNumber = Convert.ToString(ds.Tables[0].Rows[0]["LeakTestNumber"].ToString());

                    if (!string.IsNullOrEmpty(patrolNumber))
                    {
                        isExist = true;
                    }
                }

                return isExist;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetLeakTestNo : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void InsertLeakTestMaster(String LeakTestNumber, DateTime LeakTestDate, String ProdOrderNo, String SubLocationCode, 
            int LeakTestQty, String OperatorCode, String Remarks, DateTime CreatedDateTime)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spInsertLeakTestMaster]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@LeakTestNumber", LeakTestNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@LeakTestDate", LeakTestDate));
                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));

                cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));

                cmd.Parameters.Add(new SqlParameter("@LeakTestQty", LeakTestQty));
                cmd.Parameters.Add(new SqlParameter("@OperatorCode", OperatorCode));

                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));

                cmd.Parameters.Add(new SqlParameter("@CreatedDateTime", CreatedDateTime));


                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : InsertPatrolInspectionMaster() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void InsertLeakTestDetails(String LeakTestNumber, String SerialNo, int ShellLeak, int HighPressureSeatLeak,
            int LowPressureSeatLeak, int BackSeatLeak, int WeldLeak, int JointLeak, int TrapShellLeak, int TrapWeldLeak, int TrapJointLeak, int TrapShutLeak)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spInsertLeakTestDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@LeakTestNumber", LeakTestNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));

                cmd.Parameters.Add(new SqlParameter("@ShellLeak", ShellLeak));
                cmd.Parameters.Add(new SqlParameter("@HighPressureSeatLeak", HighPressureSeatLeak));

                cmd.Parameters.Add(new SqlParameter("@LowPressureSeatLeak", LowPressureSeatLeak));
                cmd.Parameters.Add(new SqlParameter("@BackSeatLeak", BackSeatLeak));

                cmd.Parameters.Add(new SqlParameter("@WeldLeak", WeldLeak));
                cmd.Parameters.Add(new SqlParameter("@JointLeak", JointLeak));

                cmd.Parameters.Add(new SqlParameter("@TrapShellLeak", TrapShellLeak));
                cmd.Parameters.Add(new SqlParameter("@TrapWeldLeak", TrapWeldLeak));
                cmd.Parameters.Add(new SqlParameter("@TrapJointLeak", TrapJointLeak));
                cmd.Parameters.Add(new SqlParameter("@TrapShutLeak", TrapShutLeak));


                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : InsertPatrolInspectionDetails() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void UpdateLeakTestMaster(String LeakTestNumber, DateTime LeakTestDate, String ProdOrderNo, int LeakTestQty, String OperatorCode, String Remarks, DateTime UpdatedDateTime)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spUpdateLeakTestMaster]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@LeakTestNumber", LeakTestNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@LeakTestDate", LeakTestDate));
                cmd.Parameters.Add(new SqlParameter("@ProdOrderNo", ProdOrderNo));

                //cmd.Parameters.Add(new SqlParameter("@SubLocationCode", SubLocationCode));

                cmd.Parameters.Add(new SqlParameter("@LeakTestQty", LeakTestQty));
                cmd.Parameters.Add(new SqlParameter("@OperatorCode", OperatorCode));

                cmd.Parameters.Add(new SqlParameter("@Remarks", Remarks));

                cmd.Parameters.Add(new SqlParameter("@UpdatedDateTime", UpdatedDateTime));


                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : UpdateLeakTestMaster() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void UpdateLeakTestDetails(String LeakTestNumber, String SerialNo, int ShellLeak, int HighPressureSeatLeak,
            int LowPressureSeatLeak, int BackSeatLeak, int WeldLeak, int JointLeak, int TrapShellLeak, int TrapWeldLeak, int TrapJointLeak, int TrapShutLeak)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spUpdateLeakTestDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@LeakTestNumber", LeakTestNumber)); // String
                cmd.Parameters.Add(new SqlParameter("@SerialNo", SerialNo));

                cmd.Parameters.Add(new SqlParameter("@ShellLeak", ShellLeak));
                cmd.Parameters.Add(new SqlParameter("@HighPressureSeatLeak", HighPressureSeatLeak));

                cmd.Parameters.Add(new SqlParameter("@LowPressureSeatLeak", LowPressureSeatLeak));
                cmd.Parameters.Add(new SqlParameter("@BackSeatLeak", BackSeatLeak));

                cmd.Parameters.Add(new SqlParameter("@WeldLeak", WeldLeak));
                cmd.Parameters.Add(new SqlParameter("@JointLeak", JointLeak));

                cmd.Parameters.Add(new SqlParameter("@TrapShellLeak", TrapShellLeak));
                cmd.Parameters.Add(new SqlParameter("@TrapWeldLeak", TrapWeldLeak));
                cmd.Parameters.Add(new SqlParameter("@TrapJointLeak", TrapJointLeak));
                cmd.Parameters.Add(new SqlParameter("@TrapShutLeak", TrapShutLeak));

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : UpdateLeakTestDetails() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetLeakTestSerial(string LeakTestNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("select SerialNo from LeakTestDetail where LeakTestNumber = '" + LeakTestNumber + "'", conn);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetLeakTestSerial() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public DataSet GetLeakTestMaster(String LeakTestNumber)
        {
            DataSet ds = new DataSet();
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spGetLeakTestMaster]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@LeakTestNumber", LeakTestNumber));

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                conn.Close();

                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : GetLeakTestMaster() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }

        public void DeleteLeakTestMasterAndDetails(String LeakTestNumber)
        {
            try
            {
                this.init();

                SqlCommand cmd = new SqlCommand("[spDeleteLeakTestMasterAndDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string UserName = (String)HttpContext.Current.Session["LoggedOnUser"];

                cmd.Parameters.Add(new SqlParameter("@LeakTestNumber", LeakTestNumber)); // String

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch (Exception ex)
            {
                conn.Close();
                Logger.Write(this.GetType().ToString() + " : DeleteLeakTestMasterAndDetails() : " + " : " + DateTime.Now + " : " + ex.Message.ToString(), Category.General, Priority.Highest);
                throw ex;
            }
        }
        #endregion


    }

    public class CustomComparer : IComparer
    {
        Comparer _comparer = new Comparer(System.Globalization.CultureInfo.CurrentCulture);

        public int Compare(object x, object y)
        {
            // Convert string comparisons to int
            return _comparer.Compare(Convert.ToInt32(x), Convert.ToInt32(y));
        }
    }
}