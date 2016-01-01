#pragma once
#include "Board.h"

namespace KAGA {
int _check(int W, int H, int bef[][16], int aft[][16], int slct) {
  //横幅，高さ，現在の状態，目標の状態，選択する（動かす）マス
  //マスを動かして目標の状態にできるかどうかの判定をする関数

  // http://www.aji.sakura.ne.jp/algorithm/slide_goal.html
  //↑これを参考にしました

  vector<int> befv, aftv;
  int cnt = 0;

  for (int i = 0; i < H; i++) {
    for (int j = ((i + H) % 2 == 0 ? W - 1 : 0); 0 <= j && j < W;
         j += ((i + H) % 2 == 0 ? -1 : 1)) {
      if (bef[i][j] != slct) befv.push_back(bef[i][j]);
      if (aft[i][j] != slct) aftv.push_back(aft[i][j]);
    }
  }

  //愚直にswapしているけど、改善できるかもしれない
  for (int i = 0; i < befv.size(); i++) {
    if (befv[i] != aftv[i]) {
      int j = i;
      while (befv[j] != aftv[i]) j++;
      while (befv[i] != aftv[i]) {
        j--;
        cnt++;
        swap(befv[j], befv[j + 1]);
      }
    }
  }
  // 1:OK/2:NO
  return (cnt + 1) % 2;
}

int check(Board &b, int select) {
  int bef[16][16], aft[16][16];
  for (int i = 0; i < b.height(); i++) {
    for (int j = 0; j < b.width(); j++) {
      bef[i][j] = b.ele(j, i);
      aft[i][j] = b.orgInd(j, i);
    }
  }
  return _check(b.width(), b.height(), bef, aft, select);
}
};
