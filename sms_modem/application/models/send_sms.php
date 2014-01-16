<?php
/**
  *
  * 可以查询已经发送的信息以及该信息的发送状态
  *
  */
  
class send_sms extends CI_Model{

	private $send_sms;

	function __construct()
	{
		// Call the model constructor
		parent::__construct();
		$this->load->database();
		date_default_timezone_set('Asia/Shanghai');
	}
	
	/**
	  * 查询发送状态
	  * 
	  * @access public
	  * @return SendStatus
	  */
	public function get_sent_status()
	{
		
		$send_task   = 't_sendtask';
		$sent_record = 't_sentrecord';
		
		// 查询条件
		$sql = 'select seqid,SentStatus,SentTime from t_sendtask join t_sentrecord  where is_ret = 0 and MsgID = TaskID';
		$query = $this->db->query($sql);
		
		if($query->num_rows() > 0)
		{
			$reports    = array();
			
			foreach($query->result_array() as $key => $row)
			{
				$reports[$key]['seqid'] 		= $row['seqid'];
				if($row['SentStatus'] >= 5 and $row['SentStatus'] <= 7)
				{
					$reports[$key]['state'] = 'sended';
					file_put_contents( 'F:\sites\sms\logs\error_log', '['.date('Y-m-d H:i:s')."] --- SentStatus: 短信发送成功\n", FILE_APPEND | LOCK_EX);
				}
				elseif($row['SentStatus'] >= 12 and $row['SentStatus'] <= 14)
				{
					$reports[$key]['state'] = 'send_failed';
					file_put_contents( 'F:\sites\sms\logs\error_log', '['.date('Y-m-d H:i:s')."] --- SentStatus: 短信发送失败\n", FILE_APPEND | LOCK_EX);
				}
				elseif($row['SentStatus'] === 3)
				{
					$reports[$key]['state'] = 'send_failed';
					file_put_contents( 'F:\sites\sms\logs\error_log', '['.date('Y-m-d H:i:s')."] --- SentStatus: 短信发送超时\n", FILE_APPEND | LOCK_EX);
				}
				
				$reports[$key]['receivdate']	= $row['SentTime'];
				
				// 将表中未查询过状态的数据更改为已查询状态
				if(empty($row['seqid']) || $row['seqid'] == 0)
				{
					file_put_contents( 'F:\sites\sms\logs\error_log', '['.date('Y-m-d H:i:s')."] --- seqid为空\n", FILE_APPEND | LOCK_EX);
				}
				
				$update_sql = 'update '.$send_task.' set is_ret=1 where seqid='.$row['seqid'];
				$ret_status = $this->db->query($update_sql);
				if($ret_status == false)
				{
					file_put_contents( 'F:\sites\sms\logs\error_log', '['.date('Y-m-d H:i:s')."] --- 更新is_ret状态失败，seqid：".$row['seqid']."\n", FILE_APPEND | LOCK_EX);
				}
				
			}
			file_put_contents('F:\sites\sms\logs\error_log', '['.date('Y-m-d H:i:s')."] --- reports: ".print_r($reports, true)."\n", FILE_APPEND | LOCK_EX);
			return $reports;
		}
		else
		{
			// 没有查询数据
			file_put_contents( 'F:\sites\sms\logs\error_log', '['.date('Y-m-d H:i:s')."] --- 没有查询到数据\n", FILE_APPEND | LOCK_EX);
			return 0;
		}
	}
	
	/**
	  * 将短信存入发送队列
	  * 
	  * @access public
	  * @return boolean
	  *
	  */
	public function insert_to_tasklist($mobile_phone, $content, $seqid)
	{
		// 插入的数据
		$data = array(
			'DestNumber'   => $mobile_phone,
			'Content' 	   => $content,
			'seqid'		   => $seqid,
			'StatusReport' => '1'
		);
		
		// 表名
		$tbl_name 	= 'T_SendTask';
		$ret_status = $this->db->insert($tbl_name, $data);
		
		
		if($ret_status)
		{
			
			file_put_contents('F:\sites\sms\logs\test.log', '['.date('Y-m-d H:i:s')."] ---  insert success\n", FILE_APPEND | LOCK_EX);
			return true;
		}
		else
		{
			// 插入失败
			file_put_contents('F:\sites\sms\logs\test.log', '['.date('Y-m-d H:i:s')."] ---  insert failed \n", FILE_APPEND | LOCK_EX);
			return false;
		}
	}
	
}
















