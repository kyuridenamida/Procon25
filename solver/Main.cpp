#include <iostream>
#include <vector>
#include <array>
#include <queue>
#include <algorithm>
#include <cstdio>
#include <cstdlib>
#include <fstream>
#include <ctime>
using namespace std;
#include "Solver.h"

int main() {
  SwapProblem problem;
  cin >> problem.width >> problem.height;
  cin >> problem.select_limit;
  cin >> problem.select_cost >> problem.move_cost;
  problem.field.resize(problem.width * problem.height);
  for (int i = 0; i < problem.height; i++) {
    for (int j = 0; j < problem.width; j++) {
      cin >> problem.field[i * problem.width + j];
    }
  }

  Solver s(problem);
  bool any_answer = false;
  bool use_select_cost = true;
  auto answer = s.solve(any_answer, use_select_cost);
  cout << answer.tostring();
  cout.flush();
}
