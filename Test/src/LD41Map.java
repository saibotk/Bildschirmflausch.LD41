import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.event.KeyEvent;
import java.awt.event.KeyListener;
import java.util.Comparator;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;
import javax.swing.JFrame;
import javax.swing.JPanel;
import org.joml.Vector2i;
import javafx.util.Pair;

public class LD41Map {

	static final int TUNNEL_THICKNESS = 3;

	public static class Vertex {

		public final Room	r;
		public float		value;
		public Vertex		parent;

		public Vertex(Room r) {
			this.r = r;
			value = Float.POSITIVE_INFINITY;
			parent = null;
		}
	}

	public static class Edge {

		public final Vertex	r1, r2;
		public double		dist;

		public Edge(Vertex r1, Vertex r2) {
			this.r1 = r1;
			this.r2 = r2;
			dist = r1.r.distance(r2.r);
		}
	}

	public static class Room {

		Rectangle				bounds	= new Rectangle();
		// int width = 0, height = 0;
		Map<Vector2i, TileType>	tiles;
		// Vector2i pos = new Vector2i();
		Set<Vector2i>			doors;

		public float distance(Room r) {
			return Math.abs(getCenter().x - r.getCenter().x) + Math.abs(getCenter().y - r.getCenter().y);
		}

		public Point getCenter() {
			return new Point((int) bounds.getCenterX(), (int) bounds.getCenterY());
		}
	}

	public static enum TileType {
		GROUND, WALL, DOOR;

	}

	public static void main(String[] args) {
		Set<Room> rooms = new HashSet<>();
		rooms.addAll(generate().getKey());

		{
			JFrame frame = new JFrame();
			frame.setDefaultCloseOperation(3);
			JPanel panel = new JPanel() {

				private static final long serialVersionUID = 5954622107064881267L;

				@Override
				public void paintComponent(Graphics h) {
					super.paintComponent(h);
					Pair<Set<Room>, Pair<Vertex, Set<Edge>>> graph = generate();
					Graphics2D g = (Graphics2D) h;
					g.translate(getWidth() / 2, getHeight() / 2);
					g.scale(4, 4);
					for (Room room : graph.getKey()) {
						g.setColor(new Color(1f, 0f, 0f, 0.5f));
						g.fill(room.bounds);
					}
				}
			};
			frame.add(panel);
			frame.setPreferredSize(new Dimension(800, 500));
			frame.pack();
			frame.setVisible(true);
			frame.repaint();
			frame.setExtendedState(6);

			frame.addKeyListener(new KeyListener() {

				@Override
				public void keyTyped(KeyEvent e) {
					rooms.clear();
					Pair<Set<Room>, Pair<Vertex, Set<Edge>>> graph = generate();
					rooms.addAll(graph.getKey());
					frame.repaint();
				}

				@Override
				public void keyReleased(KeyEvent e) {
				}

				@Override
				public void keyPressed(KeyEvent e) {
				}
			});
		}
	}

	public static Pair<Set<Room>, Pair<Vertex, Set<Edge>>> generate() {
		int minRoomSize = 50;
		Set<Room> rooms = new HashSet<>();
		for (int i = 0; i < 7 + (int) (Math.random() * 4); i++) {
			Room room = new Room();
			room.bounds.width = 15 + (int) (Math.random() * 20);
			room.bounds.height = 15 + (int) (Math.random() * 20);
			rooms.add(room);
		}

		outest: while (true) {
			for (Room r1 : rooms) {
				for (Room r2 : rooms) {
					if (r1 == r2)
						continue;
					Vector2i p1 = new Vector2i(r1.bounds.x + r1.bounds.width / 2, r1.bounds.y + r1.bounds.height / 2);
					Vector2i p2 = new Vector2i(r2.bounds.x + r2.bounds.width / 2, r2.bounds.y + r2.bounds.height / 2);
					if (p1.distanceSquared(p2) < 2 * minRoomSize * minRoomSize + 2) {
						r2.bounds.x += (int) ((Math.random() - 0.5) * 5);
						r2.bounds.y += (int) ((Math.random() - 0.5) * 2.5);
						continue outest;
					}
				}
			}
			break;
		}

		Set<Vertex> Q = new HashSet<>();
		for (Room r : rooms)
			Q.add(new Vertex(r));
		Vertex root = Q.stream().min(Comparator.comparing(v -> v.r.bounds.x)).get();
		root.value = 0;

		Set<Edge> E = new HashSet<>();
		Set<Edge> G = new HashSet<>();
		Set<Vertex> F = new HashSet<>();
		for (Vertex r1 : Q) {
			outer: for (Vertex r2 : Q) {
				if (r1 == r2)
					continue outer;
				for (Edge e : E)
					if (e.r2 == r1 && e.r1 == r2)
						continue outer;
				E.add(new Edge(r1, r2));
			}
		}
		F.add(root);
		Q.remove(root);

		while (!Q.isEmpty()) {
			Edge start = E.stream()
					.filter(e -> (F.contains(e.r1) ^ F.contains(e.r2)))
					.min(Comparator.comparing(e -> e.dist)).get();
			Q.remove(start.r2);
			Q.remove(start.r1);
			F.add(start.r2);
			F.add(start.r1);
			E.remove(start);
			G.add(start);
			if (start.r1.value < start.r2.value) {
				start.r2.value = (float) (start.r1.value + start.dist);
			} else {
				start.r1.value = (float) (start.r2.value + start.dist);
			}
		}

		// G list of edges
		// rooms list of rooms

		for (Edge ed : G) {
			// horizontal
			float diff1 = ed.r1.r.bounds.y - ed.r2.r.bounds.y - ed.r2.r.bounds.height + TUNNEL_THICKNESS;
			float diff2 = ed.r2.r.bounds.y - ed.r1.r.bounds.y - ed.r1.r.bounds.height + TUNNEL_THICKNESS;

			// vertical
			float diff3 = ed.r1.r.bounds.x - ed.r2.r.bounds.x - ed.r2.r.bounds.width + TUNNEL_THICKNESS;
			float diff4 = ed.r2.r.bounds.x - ed.r1.r.bounds.x - ed.r1.r.bounds.width + TUNNEL_THICKNESS;

			if (diff1 < 0 && diff2 < 0) {
				addStraightHorizontal(rooms, ed);
			} else if (diff3 < 0 && diff4 < 0) {
				addStraightVertical(rooms, ed);
			} else
				addCurve(rooms, ed);

		}

		return new Pair<>(rooms, new Pair<>(root, G));
	}

	public static void addStraightHorizontal(Set<Room> rooms, Edge ed) {
		Room tunnel = new Room();
		int minX = Math.min(ed.r1.r.bounds.x + ed.r1.r.bounds.width, ed.r2.r.bounds.x + ed.r2.r.bounds.width);
		int minY = Math.max(ed.r1.r.bounds.y, ed.r2.r.bounds.y);
		int maxX = Math.max(ed.r1.r.bounds.x, ed.r2.r.bounds.x);
		int maxY = Math.min(ed.r1.r.bounds.y + ed.r1.r.bounds.height, ed.r2.r.bounds.y + ed.r2.r.bounds.height);
		tunnel.bounds.x = minX;
		tunnel.bounds.y = (minY + maxY) / 2 - TUNNEL_THICKNESS / 2;
		tunnel.bounds.width = (maxX - minX);
		tunnel.bounds.height = TUNNEL_THICKNESS;

		rooms.add(tunnel);
	}

	public static void addStraightVertical(Set<Room> rooms, Edge ed) {
		Room tunnel = new Room();
		int minX = Math.max(ed.r1.r.bounds.x, ed.r2.r.bounds.x);
		int minY = Math.min(ed.r1.r.bounds.y + ed.r1.r.bounds.height, ed.r2.r.bounds.y + ed.r2.r.bounds.height);
		int maxX = Math.min(ed.r1.r.bounds.x + ed.r1.r.bounds.width, ed.r2.r.bounds.x + ed.r2.r.bounds.width);
		int maxY = Math.max(ed.r1.r.bounds.y, ed.r2.r.bounds.y);
		tunnel.bounds.x = (minX + maxX) / 2 - TUNNEL_THICKNESS / 2;
		tunnel.bounds.y = minY;
		tunnel.bounds.width = TUNNEL_THICKNESS;
		tunnel.bounds.height = (maxY - minY);

		rooms.add(tunnel);
	}

	public static void addCurve(Set<Room> rooms, Edge ed) {
		Room higher = ed.r1.r.getCenter().y > ed.r2.r.getCenter().y ? ed.r1.r : ed.r2.r;
		Room lower = ed.r1.r.getCenter().y > ed.r2.r.getCenter().y ? ed.r2.r : ed.r1.r;
		Room righter = ed.r1.r.getCenter().x > ed.r2.r.getCenter().x ? ed.r1.r : ed.r2.r;
		Room lefter = ed.r1.r.getCenter().x > ed.r2.r.getCenter().x ? ed.r2.r : ed.r1.r;

		Rectangle r = new Rectangle(lefter.getCenter().x, lower.getCenter().y, righter.getCenter().x - lefter.getCenter().x, higher.getCenter().y - lower.getCenter().y);

		Room vertical1 = new Room();
		vertical1.bounds.x = r.x - TUNNEL_THICKNESS / 2;
		vertical1.bounds.y = r.y - TUNNEL_THICKNESS / 2;
		vertical1.bounds.width = TUNNEL_THICKNESS;
		vertical1.bounds.height = r.height + TUNNEL_THICKNESS;

		Room horizontal1 = new Room();
		horizontal1.bounds.x = r.x - TUNNEL_THICKNESS / 2;
		horizontal1.bounds.y = r.y - TUNNEL_THICKNESS / 2;
		horizontal1.bounds.width = r.width + TUNNEL_THICKNESS;
		horizontal1.bounds.height = TUNNEL_THICKNESS;

		Room vertical2 = new Room();
		vertical2.bounds.x = r.x + r.width - TUNNEL_THICKNESS / 2;
		vertical2.bounds.y = r.y - TUNNEL_THICKNESS / 2;
		vertical2.bounds.width = TUNNEL_THICKNESS;
		vertical2.bounds.height = r.height + TUNNEL_THICKNESS;

		Room horizontal2 = new Room();
		horizontal2.bounds.x = r.x - TUNNEL_THICKNESS / 2;
		horizontal2.bounds.y = r.y + r.height - TUNNEL_THICKNESS / 2;
		horizontal2.bounds.width = r.width + TUNNEL_THICKNESS;
		horizontal2.bounds.height = TUNNEL_THICKNESS;

		boolean flip = Math.random() > 0.5;
		boolean diffX = ed.r2.r.getCenter().x - ed.r1.r.getCenter().x > 0;
		boolean diffY = ed.r2.r.getCenter().y - ed.r1.r.getCenter().y > 0;
		if (diffX && diffY) {
			if (flip) {
				rooms.add(vertical1);
				rooms.add(horizontal2);
			} else {
				rooms.add(vertical2);
				rooms.add(horizontal1);
			}
		} else if (diffX && !diffY) {
			if (flip) {
				rooms.add(vertical2);
				rooms.add(horizontal2);
			} else {
				rooms.add(vertical1);
				rooms.add(horizontal1);
			}
		} else if (!diffX && diffY) {
			if (flip) {
				rooms.add(vertical1);
				rooms.add(horizontal1);
			} else {
				rooms.add(vertical2);
				rooms.add(horizontal2);
			}
		} else if (!diffX && !diffY) {
			if (flip) {
				rooms.add(vertical2);
				rooms.add(horizontal1);
			} else {
				rooms.add(vertical1);
				rooms.add(horizontal2);
			}
		}
	}
}