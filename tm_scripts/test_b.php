<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class Test_b extends CI_Controller {

    function __construct() {
        parent::__construct();
        set_time_limit(0);
        ini_set('memory_limit', '512M');
    }
    function get_microtime(){ 
        list($usec, $sec) = explode(" ",microtime());
        return ((float)$usec + (float)$sec); 
    }
    
    function index()
    {
        $english_task = "\"Oh, the people will lick our boots--just as they'd lick the boots of the Germans if they entered in triumph. With them, nothing succeeds like success. They don't love the Turk, but they don't love any one but themselves. The decent Arabs, especially Firouz Ali and his little band of patriots--who've got a stronger following outside Bagdad than within--will welcome us as deliverers; but it's a very mixed population, and the most of them don't draw fine distinctions between Europeans: they're all sheep to be fleeced.Of course they don't realise what a bad time they'd have if the city became Germanised--morally, I mean, for there's no doubt that German administration would effect great material improvements. At present they're slaves to a corrupt tyranny; German tyranny is rather brutal than corrupt.They'll find that we are neither corrupt nor brutal--and take advantage of us. I'm talking as if we were already there. In the meantime you and I will be lucky if we save our skins.\"";
        $chinese_task = "“哦，那些人会舔我们的靴子--就如同德国人胜利进入后，他们会舔德国人的靴子一样。”有了他们，一事如意，万事顺利。他们不爱土耳其人，他们只爱他们自己。那些有头有脸的阿拉伯人，尤其是菲鲁·阿里和他的小帮爱国者们——他们已经在巴格达外部获得了强于内部的追随者——将欢迎作为拯救者的我们的到来；但这是个鱼龙混杂的群体，并且他们中大部分人都不能很好的区分欧洲人：他们将会是任人宰割的绵羊。当然，他们并没有意识到，如果这个城市德国化，他们的处境将十分的艰难。事实上，我的意思是，毫无疑问，德国政府将会做出巨大的实质性改善。尽管他们现在被腐败暴政奴役着，但德国的暴政却更为残忍。他们会发现我们既不腐败也不野蛮--并利用我们。我同其谈话就好像我们已经到达那里似的。在此期间，如果我们能使自己免于失败那你我就是极其幸运的。”";
        $tone = 1;
        //exec('php index.php api/task_recovery/zbybpyadawptjnrdlhsggrluqgrizwgm/mmahbkxxjocljzxdcdhjqknmoghqlxgt/34116', $result);
        
        // $url = $_SERVER['DOCUMENT_ROOT'];
        // $script = 'english_sentence_detector.sh';
        // $sh_url = "sh $url/../scripts/sentence_detector/$script \"fdaf fda .\";";
        // $result = shell_exec('LANG=en_US.utf-8; ' . $sh_url);
//         
        // var_dump($result);
        
        //$str = "3.22 HIV/AIDS subaccounts track health expenditures related to HIV/AIDS “”and are generally conducted by Ministries of Health in tandem with a general NHA estimation (for overall health). This approach allows HIV expenditures to be placed in the context of overall health care, e.g, to compute % of government health spending spent on";
        //$str = "aaa bbb. ccc ddd.";
        //$str = "Across government, we have made good“”“》》？”L^)(*()*)(_……（）——）（——+……￥…… progress decentralising power in key areas:\n …Decentralising economic power The Regional Growth Fund will provide £1.4 billion as direct support for private sector investments and some basic infrastructure.";
        //var_dump($this->memory->english_sentence_detector($str));
        echo $_SERVER['REMOTE_ADDR'];
        echo ';', date('H:i:s');
    }

    function count_num()
    {
        $time_start = $this->get_microtime();
        $this->memory->calc_sentent_word_num('en');
        //$this->memory->calc_sentent_word_num('zh_s');
        $time_end = $this->get_microtime();
        echo $time_end - $time_start;
    }

    function check_word($word, $language)
    {
        if($language === 1)
        {
            if(preg_match('/^[a-zA-Z0-9]([a-zA-Z0-9\.\- \']*[a-zA-Z0-9])?$/', $word))
            {
                return TRUE;
            }
            else
            {
                return FALSE;
            }
        }
        else
        {
            if(preg_match('/^[\x{4e00}-\x{9fa5}a-zA-Z0-9]+$/u', $word))
            {
                return TRUE;
            }
            else
            {
                return FALSE;
            }
        }
    }


    function show_words($num = 0)
    {
        $content = '';
        $step = 100;
        $start = $num * $step;
        for ($i = $start; $i < $start + $step; $i ++)
        {
            $file = '../uploads/phrase/output_' . $i . '.txt';
            if(!file_exists($file))
            {
                continue;
            }
            $fp = fopen($file, 'r'); 
            $content .= fread($fp, filesize($file));
            fclose($fp);
        }
        $this->load->view('test_page', array('content' => $content));
    }
    function store_words()
    {
        $source_word = $this->input->post('source');
        
        $target_words = $this->input->post('target');
        foreach ($target_words as $target_word)
        {
            //$this->insert_dic_word($source_word, $target_word, 1);
        }
        echo 'success';
    }

    /**
     * Insert the translation memory dic word to db
     * @param string $english_word
     * @param string $chinese_word
     * @param int $industry_id
     */
    function insert_dic_word($english_word, $chinese_word, $industry_id)
    {
        //echo $english_word, ':', $chinese_word, '<br />';
        if(preg_match('/[a-z]/', $english_word))
        {
            $english_word = strtolower($english_word);
        }
        if(!$this->check_word($english_word, 1) || !$this->check_word($chinese_word, 2))
        {
            return;
        }
        
        $sql = "SELECT `id` FROM `memory_en_word` WHERE `word` = ?";
        $query = $this->db->query($sql, array($english_word));
        if($query->num_rows() > 0)
        {
            $english_word_id = $query->row()->id;
        }
        else
        {
            $sql = "INSERT INTO `memory_en_word` (`word`) VALUES (?)";
            $this->db->query($sql, array($english_word));
            $english_word_id = $this->db->insert_id();
        }
        
        $sql = "SELECT `id` FROM `memory_cn_word` WHERE `word` = ?";
        $query = $this->db->query($sql, array($chinese_word));
        if($query->num_rows() > 0)
        {
            $chinese_word_id = $query->row()->id;
        }
        else
        {
            $sql = "INSERT INTO `memory_cn_word` (`word`) VALUES (?)";
            $this->db->query($sql, array($chinese_word));
            $chinese_word_id = $this->db->insert_id();
        }
        

        $sql = "SELECT `id`
                FROM `memory_en_cn_word_relation`
                WHERE `industry_id` = ?
                AND `en_word_id` = ?
                AND `cn_word_id` = ?";
        $query = $this->db->query($sql, array($industry_id, $english_word_id, $chinese_word_id));
        if($query->num_rows() === 0)
        {
            $sql = "INSERT INTO `memory_en_cn_word_relation` (`industry_id`, `en_word_id`, `cn_word_id`) VALUES (?, ?, ?)";
            $this->db->query($sql, array($industry_id, $english_word_id, $chinese_word_id));
        }
    }
    
    
    function insert_word_from_file()
    {
        $fp = fopen('../uploads/dic.txt', 'r'); 
        $content = fread($fp, filesize('../uploads/dic.txt'));
        //print_r(explode("\n", $content));
        fclose($fp);
        $words = explode("\n", $content);
        foreach ($words as $key => $word)
        {
            // if($key >= 100)
            // {
                // break;
            // }
            preg_match("/^\S+/u", $word, $temp_word);
            // echo $temp_word[0], "\n<br />";
            $this->insert_word(trim($temp_word[0]), 2);
        }
    }
    function insert_word($word, $language)
    {
        if($language === 1)
        {
            $table = 'memory_en_word';
        }
        else
        {
            $table = 'memory_cn_word';
        }
        $sql = "SELECT `id` FROM `$table` WHERE `word` = ?";
        $query = $this->db->query($sql, array($word));
        if($query->num_rows() === 0)
        {
            $sql = "INSERT INTO `$table` (`word`) VALUES (?)";
            $this->db->query($sql, array($word));
        }
    }
}