css-
====

有时候用git合并的时候 css冲突了，解决起来非常麻烦。  这个程序可以自动找出重复的并合并。

例如原文home.css内容
.test1 {
  background-position: -163px -7px;
  right: 6px;
  top: 4px;
}
.test2 {
  background-position: -163px -7px;
  right: 6px;
  top: 4px;
}
.test1 {
  background-position: -163px -7px;
  right: 6px;
  top: 4px;
}
合并后
.test1, .test2 {
    background-position: -163px -7px;
    right: 6px;
    top: 4px;
}

home.css中是需要压缩的原文。  运行程序后压缩后内容输出在newCss.css中
