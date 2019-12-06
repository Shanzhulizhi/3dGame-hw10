using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;



public class AIState {

	//左右岸的数量
    private int leftPriests;
    private int leftDevils;
    private int rightPriests;
    private int rightDevils;
    //true为左岸，false为右岸
    private bool location;
    private AIState last;


    public AIState(int leftPriests, int leftDevils, int rightPriests, int rightDevils, bool location, AIState last) {
      this.leftPriests = leftPriests;
      this.leftDevils = leftDevils;
      this.rightPriests = rightPriests;
      this.rightDevils = rightDevils;
      this.location = location;
      this.last = last;
    }

    public AIState(AIState a) {
      this.leftPriests = a.leftPriests;
      this.leftDevils = a.leftDevils;
      this.rightPriests = a.rightPriests;
      this.rightDevils = a.rightDevils;
      this.location = a.location;
      this.last = a.last;
    }

   	public int getLP(){return leftPriests;}
    public int getRP(){return rightPriests;}
    public int getLD(){return leftDevils;}
    public int getRD(){return rightDevils;}
    public bool getLoc(){return location;}
    public AIState getLast(){return last;}

    public void setLP(int lp){leftPriests = lp;}
    public void setRP(int rp){rightPriests = rp;}
    public void setLD(int ld){leftDevils = ld;}
    public void setRD(int rd){rightDevils = rd;}
    public void setLoc(bool l){location = l;}
    public void setLast(AIState a){last = a;}

    public static bool operator ==(AIState lhs, AIState rhs) {
      return (lhs.getLP() == rhs.getLP() && lhs.getLD() == rhs.getLD() &&
        lhs.getRP() == rhs.getRP() && lhs.getRD() == rhs.getRD() &&
        lhs.getLoc() == rhs.getLoc());
    }

    public static bool operator !=(AIState lhs, AIState rhs) {
      return !(lhs == rhs);
    }

   	//判断状态是否合法
    public bool isValid() {
      return ((this.leftPriests == 0 || this.leftPriests >= this.leftDevils) &&
        (this.rightPriests == 0 || this.rightPriests >= this.rightDevils));
    }


    public override bool Equals(object obj) {
      if (obj == null) {
        return false;
      }
      if (obj.GetType().Equals(this.GetType()) == false) {
        return false;
      }
      AIState temp = null;
      temp = (AIState)obj;
      return this.leftPriests.Equals(temp.getLP()) &&
        this.leftDevils.Equals(temp.getLD()) &&
        this.rightDevils.Equals(temp.getRD()) &&
        this.rightPriests.Equals(temp.getRP()) &&
        this.location.Equals(temp.getLoc());
    }

    public override int GetHashCode() {
      return this.leftDevils.GetHashCode() + this.leftPriests.GetHashCode() +
        this.rightDevils.GetHashCode() + this.rightPriests.GetHashCode() +
        this.location.GetHashCode();
    }

    public static AIState BFS(AIState start, AIState end) {

  		Queue<AIState> queue = new Queue<AIState>();
		Queue<AIState> visited = new Queue<AIState>();
		AIState temp = new AIState(start.getLP(), start.getLD(), start.getRP(), start.getRD(), start.getLoc(), null);
		queue.Enqueue(temp);

		while (queue.Count > 0) {
			temp = queue.Peek();

			if (temp == end) {
				while (temp.last != start) {
					temp = temp.last;
				}
				return temp;
			}

			queue.Dequeue();
			visited.Enqueue(temp);

			//从左到右
			if (temp.getLoc()) {

				//移动一个牧师
				if (temp.getLP() > 0) {
					AIState next = new AIState(temp.getLP()-1,temp.getLD(),temp.getRP()+1,temp.getRD(),false,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
						queue.Enqueue(next);
					}
				}

				//移动一个魔鬼
				if (temp.getLD() > 0) {
					AIState next = new AIState(temp.getLP(),temp.getLD()-1,temp.getRP(),temp.getRD()+1,false,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
					  queue.Enqueue(next);
					}
				}

				//移动一个牧师与一个魔鬼
				if (temp.getLD() > 0 && temp.getLP() > 0) {
					AIState next = new AIState(temp.getLP()-1,temp.getLD()-1,temp.getRP()+1,temp.getRD()+1,false,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
					  queue.Enqueue(next);
					}
				}

				//移动两个牧师
				if (temp.getLP() > 1) {
					AIState next = new AIState(temp.getLP()-2,temp.getLD(),temp.getRP()+2,temp.getRD(),false,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
					  queue.Enqueue(next);
					}
				}

				//移动两个魔鬼
				if (temp.getLD() > 1) {
					AIState next = new AIState(temp.getLP(),temp.getLD()-2,temp.getRP(),temp.getRD()+2,false,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
					  queue.Enqueue(next);
					}
				}
    		} 

    		//从右到左
		    else {

				//移动一个牧师
				if (temp.getRP() > 0) {
					AIState next = new AIState(temp.getLP()+1,temp.getLD(),temp.getRP()-1,temp.getRD(),true,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
				  		queue.Enqueue(next);
					}
				}

				//移动一个魔鬼
				if (temp.getRD() > 0) {
					AIState next = new AIState(temp.getLP(),temp.getLD()+1,temp.getRP(),temp.getRD()-1,true,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
				  		queue.Enqueue(next);
					}
				}

				//移动一个牧师和一个魔鬼
				if (temp.getRD() > 0 && temp.getRP() > 0) {
					AIState next = new AIState(temp.getLP()+1,temp.getLD()+1,temp.getRP()-1,temp.getRD()-1,true,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
				  		queue.Enqueue(next);
					}
				}

				//移动两个魔鬼
				if (temp.getRD() > 1) {
					AIState next = new AIState(temp.getLP(),temp.getLD()+2,temp.getRP(),temp.getRD()-2,true,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
				  		queue.Enqueue(next);
					}
				}

				//移动两个牧师
				if (temp.getRP() > 1) {
					AIState next = new AIState(temp.getLP()+2,temp.getLD(),temp.getRP()-2,temp.getRD(),true,temp);
					if (next.isValid() && !visited.Contains(next) && !queue.Contains(next)) {
					  queue.Enqueue(next);
					}
				}
	    	}
		}
	return null;
    }
}

