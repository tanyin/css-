<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');
/**
 * 从common_server接受post消息
 * 然后转发给短信猫发送
 * 发送完成后，讲发送状态返回给common_server中。
 *
 */
class sms_message extends CI_Controller {

	private $error_log = 'F:\sites\sms\logs\error_log';

	public function index()
	{
		phpinfo();
	}

	/**
	  * 调用短信猫发送短信，并将发送状态返回给common_server
	  * 
	  * @access public
	  * @return boolean
	  *
	  */
	public function send_message()
	{
		$this->_check_log();

		// 载入model
		$this->load->model('send_sms');
	
		// 设置时间
		date_default_timezone_set('Asia/Shanghai');
		set_time_limit(0);
		
		$message = json_decode($this->input->post('messages'), true);
		
		// 测试用语句
		// file_put_contents('F:\sites\sms\logs\test.log', '['.date('Y-m-d H:i:s').'] --- '.print_r($message, true)."  ---\n", FILE_APPEND | LOCK_EX);
		// return false;
		
		if(empty($message))
		{
			file_put_contents($this->error_log, '['.date('Y-m-d H:i:s').'] --- '.print_r($message, true)."\n", FILE_APPEND | LOCK_EX);
			echo json_encode(false);
		}
				
		// 将信息加入发送队列，返回MsgID以便查询信息状态
		$ret_status   = $this->send_sms->insert_to_tasklist($message['mobile_phone'], $message['content'], $message['seqid']);

		if($ret_status === FALSE)
		{
			file_put_contents($this->error_log, '['.date('Y-m-d H:i:s')."] --- 向短信队列插入信息失败\n", FILE_APPEND | LOCK_EX);
			echo json_encode(false);
		}
		
		echo json_encode(true);
		
	}
	
	/**
	  * 获取短信发送状态
	  * 
	  * @access public
	  * @return array
	  *
	  */
	  public function get_sms_status()
	  {
		// 载入model
		$this->load->model('send_sms');
		
		$reports = $this->send_sms->get_sent_status();
		
		if(empty($reports) || !$reports)
		{
			return json_encode(false);
		}
		file_put_contents($this->error_log, '['.date('Y-m-d H:i:s')."] --- new reports: ".print_r($reports, true)."\n", FILE_APPEND | LOCK_EX);
		echo json_encode($reports);
	  }
	
	/**
	  * 让每天的log中间有分割线
	  *
	  * @access private
	  * @return boolean
	  *
	  */
	private function _check_log()
	{
		// 设定时区
		date_default_timezone_set('Asia/Shanghai');
		// 获取当前时间
		$today_time = mktime(0, 0, 0, date('m'), date('d'), date('Y'));
		
		// 获取log修改时间
		$modify_time = fileatime($this->error_log);
		
		// 如果log修改时间小于当前时间，证明这次写入是今天第一条，所以需要分割线
		if($modify_time < $today_time)
		{
			file_put_contents($this->error_log, '\n\n\n=======================================================  ['.date('Y-m-d')."]log开始  ==========================================================================\n\n\n", FILE_APPEND | LOCK_EX);
		}
	}
	
}

/* End of file welcome.php */
/* Location: ./application/controllers/welcome.php */