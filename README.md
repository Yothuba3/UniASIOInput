# Warning  
本アセットは安定板ではありません．AsioドライバによってはInput取得中それ以外のアプリケーションからのアウトプット， インプットすべてを受け付けなくなるAsioドライバもあります.  
本アセットは，非常に限定的な用途での利用を想定しているため，これらの検証，調整は行われない可能性があります．    
上記の回避策として，Unity上でAsioManagerがドライバを取得した後に，windowsの出力を別の出力に変更後，再度オーディオインターフェースを指定することで，音声が復活する可能性があります．  
![image](https://user-images.githubusercontent.com/39334911/161339407-5a3a0681-bbbb-4490-9cba-08778c55f87c.png)

# Overview  
本アセットは，UnityでASIOによるオーディオ入力をサポートするものです．  
ベースにはNAuidoライブラリを利用しています．  
Unityの標準機能ではサポートされていない，オーディオインターフェースからの  
マルチチャンネルインプットに対応しています．

# How to Install
本アセットは，PackageManagerを経由して提供されます．
https://docs.unity3d.com/ja/2019.4/Manual/upm-ui-giturl.html

# How to Use  
## Class  
- ASIOManager
  - Asioドライバを指定することで後述のUniAsioReceiverで利用可能になります．
  - Buffer Size Per Ch，Samplerateは利用するAsioドライバの設定と合わせてください．
  - ![image](https://user-images.githubusercontent.com/39334911/161339731-73a8d66b-76fe-4d98-9cf1-01b0984299d8.png)

- UniAsioReceiver
  - Asioドライバから指定したチャンネルのインプットを取得します．
  - 取得される値は時間軸的なオーディオデータであり，-1~1の範囲になっています．
