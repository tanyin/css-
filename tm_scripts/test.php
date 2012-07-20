<?php if ( ! defined('BASEPATH')) exit('No direct script access allowed');

class Test extends CI_Controller {

    function __construct() {
        parent::__construct();
        $this->load->model('translation/translation_memory_model');
        $this->load->library('memory', array('server_address'=>'localhost','server_port'=>9312));
        set_time_limit(0);
        ini_set('memory_limit', '1000M');
        
        
        
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
        echo ';', date('H:i:s'), '<br />';
        //print_r($this->memory->tokenizer($chinese_task, 'zh_s'));
        print_r($this->memory->get_word_category_translation('联合国人口基金', 2, 2, 1));
    }
    function finished_task_recovery($start = 0, $step = 0)
    {
        $time_start = $this->get_microtime();
        // $sql = "SELECT `translation_project`.`id`, `translation_project_category`.`category_id`, `translation_project`.`source_lang`, `translation_project`.`target_lang`, `translation_project`.`industry_id`
                // FROM `translation_project`
                // INNER JOIN `translation_project_category` ON `translation_project_category`.`project_id` = `translation_project`.`id`
                // INNER JOIN `category` ON `category`.`id` = `translation_project_category`.`category_id`
                // WHERE `translation_project`.`type` NOT IN ('disused', 'translation', 'new')
                // AND `translation_project`.`file_type` != 'xml'
                // AND `category`.`type` = 'tone'
                // ORDER BY `translation_project`.`id`";
        $sql = "SELECT `translation_project`.`id`, `translation_project_category`.`category_id`, `translation_project`.`source_lang`, `translation_project`.`target_lang`, `translation_project`.`industry_id`
                FROM `translation_project`
                INNER JOIN `translation_project_category` ON `translation_project_category`.`project_id` = `translation_project`.`id`
                INNER JOIN `category` ON `category`.`id` = `translation_project_category`.`category_id`
                WHERE `translation_project`.`id` IN ('333', '391', '392')
                AND `category`.`type` = 'tone'
                ORDER BY `translation_project`.`id`";
        $temp_project_array = $this->db->query($sql)->result_array();
        
        $special_project_array = array(174, 194, 196, 197, 198);
        foreach($temp_project_array as $project)
        {
            $project_array[$project['id']]['category_id'] = $project['category_id'];
            $project_array[$project['id']]['industry_id'] = $project['industry_id'];
            if(in_array($project['id'], $special_project_array))
            {
                $project_array[$project['id']]['source_lang'] = $project['target_lang'];
                $project_array[$project['id']]['target_lang'] = $project['source_lang'];
            }
            else
            {
                $project_array[$project['id']]['source_lang'] = $project['source_lang'];
                $project_array[$project['id']]['target_lang'] = $project['target_lang'];
            }
        }
        
        $sql = "SELECT `translation_task`.`project_id`, `translation_task`.`content` AS `original_content`, `user_translation_task`.`content` AS 'translated_content'
                FROM `translation_task`
                INNER JOIN `user_translation_task` ON `user_translation_task`.`id` = `translation_task`.`latest_user_translation_task_id`
                WHERE `translation_task`.`status` IN ('user_finished', 'admin_pick_finished', 'admin_trans_finished', 'admin_repick_finished')";
        $task_array = $this->db->query($sql)->result_array();
        
        $count = 0;
        echo $start + 1, ' to ', $start + $step, '<br />';
        foreach($task_array as $task)
        {
            if(isset($project_array[$task['project_id']]))
            {
                $count++;
                if($count <= $start)
                {
                    continue;
                }
                if($count > ($start + $step))
                {
                    break;
                }
                if($project_array[$task['project_id']]['source_lang'] == 1)
                {
                    $this->translation_memory_model->task_recovery($task['original_content'], $task['translated_content'], $project_array[$task['project_id']]['category_id']);
                }
                else
                {
                    $this->translation_memory_model->task_recovery($task['translated_content'], $task['original_content'], $project_array[$task['project_id']]['category_id']);
                }
            }
        }
        $time_end = $this->get_microtime();
        echo $time_end - $time_start, '<br />';
        echo memory_get_usage()/1024/1024, 'MB';
    }

    function count_num()
    {
        $time_start = $this->get_microtime();
        $this->memory->calc_sentent_word_num('en');
        //$this->memory->calc_sentent_word_num('zh_s');
        $time_end = $this->get_microtime();
        echo $time_end - $time_start;
    }
    
    function word_recovery()
    {
        ini_set('memory_limit', '512M');
        $time_start = $this->get_microtime();
        $sql = "SELECT `memory_en_corpus`.`text` AS `en_corpus`, `memory_cn_corpus`.`text` AS `cn_corpus`, `memory_en_cn_corpus_relation`.`translated_times`
                FROM `memory_en_cn_corpus_relation`
                INNER JOIN `memory_en_corpus` ON `memory_en_corpus`.`id` = `memory_en_cn_corpus_relation`.`en_corpus_id`
                INNER JOIN `memory_cn_corpus` ON `memory_cn_corpus`.`id` = `memory_en_cn_corpus_relation`.`cn_corpus_id`";
        $sentences = $this->db->query($sql)->result_array();
        foreach($sentences as $sentence)
        {
            $english_words = $this->memory->english_tokenizer($sentence['en_corpus']);
            $chinese_words = $this->memory->chinese_tokenizer($sentence['cn_corpus']);
            foreach($english_words as $english_word => $english_times)
            {
                foreach($chinese_words as $chinese_word => $chinese_times)
                {
                    if($english_times >= $chinese_times)
                    {
                        $this->translation_memory_model->insert_word($english_word, $chinese_word, $sentence['translated_times'] * $english_times, 1);
                    }
                }
            }
        }
        $time_end = $this->get_microtime();
        echo $time_end - $time_start, '   ';
        echo memory_get_usage()/1024/1024, 'MB';
    }
    function test1()
    {
        echo $this->translation_memory_model->get_cat_translation('That\'s banking on after ancient metaphors.', 'en', 6, 1);
    }
    function migration_words($num)
    {
        $step = 20000;
        $start = $num * $step;
        $sql = "SELECT `id`, `word` FROM `memory_en_word` ORDER BY `id` LIMIT $start, $step";
        $en_words_array = $this->db->query($sql)->result_array();
        
        $relation_sql = "SELECT `cn_word_id` FROM `memory_en_cn_word_relation` WHERE `en_word_id` = ?";
        $cn_word_sql = "SELECT `word` FROM `memory_cn_word` WHERE `id` = ?";
        foreach ($en_words_array as $en_word)
        {
            $cn_words_array = $this->db->query($relation_sql, array($en_word['id']))->result_array();
            foreach ($cn_words_array as $cn_word)
            {
                $this->insert_dic_word($en_word['word'], $this->db->query($cn_word_sql, array($cn_word['cn_word_id']))->row()->word, 1);
            }
        }
        echo memory_get_usage()/1024/1024, 'MB';
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
    function pspell()
    {
        
        echo $this->spellCheck('PHP is a reflecktive proegramming langwage origenaly dezigned for prodewcing dinamic waieb pagges.');
    }
        
    function spellCheckWord($word) {
        $pspell = pspell_new('en','','','utf-8',PSPELL_FAST);
        $autocorrect = TRUE;
    
        // Take the string match from preg_replace_callback's array
        $word = $word[0];
        
        // Ignore ALL CAPS
        if (preg_match('/^[A-Z]*$/',$word)) return $word;
    
        // Return dictionary words
        if (pspell_check($pspell,$word))
        {
            return $word;
        }
    
        // Auto-correct with the first suggestion, color green
        if ($autocorrect && $suggestions = pspell_suggest($pspell,$word))
        {
            return '<span style="color:#00FF00;">'.current($suggestions).'</span>';
        }
        
        // No suggestions, color red
        return '<span style="color:#FF0000;">'.$word.'</span>';
    }
    
    function spellCheck($string)
    {
        return preg_replace_callback('/\b\w+\b/','self::spellCheckWord',$string);
    }
    function api_test($task_id)
    {
        $this->config->load('zodo');
        $api_url = $this->config->item('api_url');
        exec('wget ' . $api_url . '/api/task_recovery/zbybpyadawptjnrdlhsggrluqgrizwgm/mmahbkxxjocljzxdcdhjqknmoghqlxgt/' . $task_id . ' > /dev/null &');
    }
    function show_words($num = 0)
    {
        $content = '';
        $step = 50;
        $start = $num * $step;
        for ($i = $start; $i < $start + $step; $i ++)
        {
            $file = '../uploads/temp/output_' . $i . '.txt';
            if(!file_exists($file))
            {
                continue;
            }
            if(file_exists($file))
            {
                $fp = fopen($file, 'r');
                $content .= fread($fp, filesize($file));
                fclose($fp);
            }
        }
        $this->load->view('test_page', array('content' => $content));
    }
    function store_words()
    {
        $source_word = $this->input->post('source');
        
        $target_words = $this->input->post('target');
        foreach ($target_words as $target_word)
        {
            $this->insert_dic_word($source_word, $target_word, 1);
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
        $fp = fopen('../uploads/input.txt', 'r'); 
        $content = fread($fp, filesize('../uploads/input.txt'));
        //print_r(explode("\n", $content));
        fclose($fp);
        $words = explode("\n", $content);
        foreach ($words as $key => $word)
        {
            // if($key >= 100)
            // {
                // break;
            // }
            //preg_match("/^\S+/u", $word, $temp_word);
            echo trim($word), "\n<br />";
            //$this->temp_insert_word(trim($temp_word[0]), 2);
            $this->temp_insert_word(trim($word), 2);
        }
    }
    function temp_insert_word($word, $language)
    {
        if(!$this->check_word($word, $language))
        {
            return;
        }
        $sql = "INSERT IGNORE `temp_word` (`word`) VALUES (?)";
        $this->db->query($sql, array($word));
    }
    function insert_word($word, $language)
    {
        if($language === 1)
        {
            if(preg_match('/[a-z]/', $word))
            {
                $word = strtolower($word);
            }
            $table = 'memory_en_word';
        }
        else
        {
            $table = 'memory_cn_word';
        }
        if(!$this->check_word($word, $language))
        {
            return;
        }
        $sql = "SELECT `id` FROM `$table` WHERE `word` = ?";
        $query = $this->db->query($sql, array($word));
        if($query->num_rows() === 0)
        {
            $sql = "INSERT INTO `$table` (`word`) VALUES (?)";
            $this->db->query($sql, array($word));
        }
    }
    function pattern()
    {
        $str = 'I: 采访者';
        $pattern = '/(((\x{2026}\x{2026}\x{201D})|(\x{3002}\x{201D})|(\x{FF1F}\x{201D})|((\x{FF01}\x{201D})(\x{2026}\x{201D}))|(\x{FF1B}x{201D})|(\x{2026}\x{2026})|\x{3002}|\x{FF1F}|\x{FF01}|\x{2026}|\x{FF1B})([^\S\n]*\n+)?)/u';
        $original_content = preg_replace($pattern, "\\1*@*",$str);
        print_r($original_content);         
        
    }
    function get_word_translation($word, $industry_id, $language_id)
    {
        echo $this->translation_memory_model->get_word_translation($word, 1, 1), "\n";
    }
}
