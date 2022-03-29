# Overview  
本アセットは，UnityでASIOによるオーディオ入力をサポートするものです．  
ベースにはNAuidoライブラリを利用しています．  
Unityの標準機能ではサポートされていない，オーディオインターフェースからの  
マルチチャンネルインプットに対応しています．

# How to Install
本アセットは，PackageManagerを経由して提供されます．
他のアセットをインストールするのとの同じ方法でgitURLを指定すればインストールできます．  

# How to Use  
## Class  
- ASIOManager
  - Asioドライバを指定することで後述のAsioAudioSyncで利用可能になります．
- ASIOAudioSync
  - Asioドライバから指定したチャンネルのインプットを取得します．
  - 取得される値は時間軸的なオーディオデータであり，0~1に正規化されています．